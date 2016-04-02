using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace NAppUpdate.SampleApp
{
	// from http://www.thomaslevesque.com/2011/03/27/wpf-display-an-animated-gif-image/
	// also see http://stackoverflow.com/questions/210922/how-do-i-get-an-animated-gif-to-work-in-wpf
	public static class ImageBehavior
	{
		#region AnimatedSource

		[AttachedPropertyBrowsableForType(typeof(Image))]
		public static ImageSource GetAnimatedSource(Image obj)
		{
			return (ImageSource)obj.GetValue(AnimatedSourceProperty);
		}

		public static void SetAnimatedSource(Image obj, ImageSource value)
		{
			obj.SetValue(AnimatedSourceProperty, value);
		}

		public static readonly DependencyProperty AnimatedSourceProperty =
			DependencyProperty.RegisterAttached(
			  "AnimatedSource",
			  typeof(ImageSource),
			  typeof(ImageBehavior),
			  new UIPropertyMetadata(
				null,
				AnimatedSourceChanged));

		private static void AnimatedSourceChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			Image imageControl = o as Image;
			if (imageControl == null)
				return;

			var oldValue = e.OldValue as ImageSource;
			var newValue = e.NewValue as ImageSource;
			if (oldValue != null)
			{
				imageControl.BeginAnimation(Image.SourceProperty, null);
			}
			if (newValue != null)
			{
				imageControl.DoWhenLoaded(InitAnimationOrImage);
			}
		}

		private static void InitAnimationOrImage(Image imageControl)
		{
			BitmapSource source = GetAnimatedSource(imageControl) as BitmapSource;
			if (source != null)
			{
				var decoder = GetDecoder(source) as GifBitmapDecoder;
				if (decoder != null && decoder.Frames.Count > 1)
				{
					var animation = new ObjectAnimationUsingKeyFrames();
					var totalDuration = TimeSpan.Zero;
					BitmapSource prevFrame = null;
					FrameInfo prevInfo = null;
					foreach (var rawFrame in decoder.Frames)
					{
						var info = GetFrameInfo(rawFrame);
						var frame = MakeFrame(
							source,
							rawFrame, info,
							prevFrame, prevInfo);

						var keyFrame = new DiscreteObjectKeyFrame(frame, totalDuration);
						animation.KeyFrames.Add(keyFrame);

						totalDuration += info.Delay;
						prevFrame = frame;
						prevInfo = info;
					}
					animation.Duration = totalDuration;
					animation.RepeatBehavior = RepeatBehavior.Forever;
					if (animation.KeyFrames.Count > 0)
						imageControl.Source = (ImageSource)animation.KeyFrames[0].Value;
					else
						imageControl.Source = decoder.Frames[0];
					imageControl.BeginAnimation(Image.SourceProperty, animation);
					return;
				}
			}
			imageControl.Source = source;
			return;
		}

		private static BitmapDecoder GetDecoder(BitmapSource image)
		{
			BitmapDecoder decoder = null;
			var frame = image as BitmapFrame;
			if (frame != null)
				decoder = frame.Decoder;

			if (decoder == null)
			{
				var bmp = image as BitmapImage;
				if (bmp != null)
				{
					if (bmp.StreamSource != null)
					{
						bmp.StreamSource.Position = 0;
						decoder = BitmapDecoder.Create(bmp.StreamSource, bmp.CreateOptions, bmp.CacheOption);
					}
					else if (bmp.UriSource != null)
					{
						Uri uri = bmp.UriSource;
						if (bmp.BaseUri != null && !uri.IsAbsoluteUri)
							uri = new Uri(bmp.BaseUri, uri);
						decoder = BitmapDecoder.Create(uri, bmp.CreateOptions, bmp.CacheOption);
					}
				}
			}

			return decoder;
		}

		private static BitmapSource MakeFrame(
			BitmapSource fullImage,
			BitmapSource rawFrame, FrameInfo frameInfo,
			BitmapSource previousFrame, FrameInfo previousFrameInfo)
		{
			DrawingVisual visual = new DrawingVisual();
			using (var context = visual.RenderOpen())
			{
				if (previousFrameInfo != null && previousFrame != null &&
					previousFrameInfo.DisposalMethod == FrameDisposalMethod.Combine)
				{
					var fullRect = new Rect(0, 0, fullImage.PixelWidth, fullImage.PixelHeight);
					context.DrawImage(previousFrame, fullRect);
				}

				context.DrawImage(rawFrame, frameInfo.Rect);
			}
			var bitmap = new RenderTargetBitmap(
				fullImage.PixelWidth, fullImage.PixelHeight,
				fullImage.DpiX, fullImage.DpiY,
				PixelFormats.Pbgra32);
			bitmap.Render(visual);
			return bitmap;
		}

		private class FrameInfo
		{
			public TimeSpan Delay { get; set; }
			public FrameDisposalMethod DisposalMethod { get; set; }
			public double Width { get; set; }
			public double Height { get; set; }
			public double Left { get; set; }
			public double Top { get; set; }

			public Rect Rect
			{
				get { return new Rect(Left, Top, Width, Height); }
			}
		}

		private enum FrameDisposalMethod
		{
			Replace = 0,
			Combine = 1,
			RestoreBackground = 2,
			RestorePrevious = 3
		}

		private static FrameInfo GetFrameInfo(BitmapFrame frame)
		{
			var frameInfo = new FrameInfo
			{
				Delay = TimeSpan.FromMilliseconds(100),
				DisposalMethod = FrameDisposalMethod.Replace,
				Width = frame.PixelWidth,
				Height = frame.PixelHeight,
				Left = 0,
				Top = 0
			};

			BitmapMetadata metadata;
			try
			{
				metadata = frame.Metadata as BitmapMetadata;
				if (metadata != null)
				{
					const string delayQuery = "/grctlext/Delay";
					const string disposalQuery = "/grctlext/Disposal";
					const string widthQuery = "/imgdesc/Width";
					const string heightQuery = "/imgdesc/Height";
					const string leftQuery = "/imgdesc/Left";
					const string topQuery = "/imgdesc/Top";

					var delay = metadata.GetQueryOrNull<ushort>(delayQuery);
					if (delay.HasValue)
						frameInfo.Delay = TimeSpan.FromMilliseconds(10 * delay.Value);

					var disposal = metadata.GetQueryOrNull<byte>(disposalQuery);
					if (disposal.HasValue)
						frameInfo.DisposalMethod = (FrameDisposalMethod)disposal.Value;

					var width = metadata.GetQueryOrNull<ushort>(widthQuery);
					if (width.HasValue)
						frameInfo.Width = width.Value;

					var height = metadata.GetQueryOrNull<ushort>(heightQuery);
					if (height.HasValue)
						frameInfo.Height = height.Value;

					var left = metadata.GetQueryOrNull<ushort>(leftQuery);
					if (left.HasValue)
						frameInfo.Left = left.Value;

					var top = metadata.GetQueryOrNull<ushort>(topQuery);
					if (top.HasValue)
						frameInfo.Top = top.Value;
				}
			}
			catch (NotSupportedException)
			{
			}

			return frameInfo;
		}

		private static T? GetQueryOrNull<T>(this BitmapMetadata metadata, string query)
			where T : struct
		{
			if (metadata.ContainsQuery(query))
			{
				object value = metadata.GetQuery(query);
				if (value != null)
					return (T)value;
			}
			return null;
		}

		#endregion
	}

	#region Extension Methods Class
	public static class ImageBehaviorExtensionMethods
	{
		public static void DoWhenLoaded<T>(this T element, Action<T> action)
		where T : FrameworkElement
		{
			if (element.IsLoaded)
			{
				action(element);
			}
			else
			{
				RoutedEventHandler handler = null;
				handler = (sender, e) =>
				{
					element.Loaded -= handler;
					action(element);
				};
				element.Loaded += handler;
			}
		}
	}
	#endregion

}
