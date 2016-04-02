using Microsoft.VisualStudio.TestTools.UnitTesting;
using NAppUpdate.Framework.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NAppUpdate.Tests.Conditions
{
	internal class MockCondition : NAppUpdate.Framework.Conditions.IUpdateCondition
	{
		private bool _isMet;

		internal MockCondition(bool isMet)
		{
			_isMet = isMet;
		}

		public bool IsMet(Framework.Tasks.IUpdateTask task)
		{
			return _isMet;
		}
	}

	[TestClass]
	public class BooleanConditionTests
	{
		[TestMethod]
		public void ShortCircuitOR()
		{
			BooleanCondition bc = new BooleanCondition();
			bc.AddCondition(new MockCondition(true), BooleanCondition.ConditionType.OR);
			bc.AddCondition(new MockCondition(true), BooleanCondition.ConditionType.OR);
			bc.AddCondition(new MockCondition(false), BooleanCondition.ConditionType.OR);

			bool isMet = bc.IsMet(null);

			Assert.IsTrue(isMet, "Expected the second or to short circuit the condition list");
		}

		[TestMethod]
		public void MultipleAND()
		{
			BooleanCondition bc = new BooleanCondition();
			bc.AddCondition(new MockCondition(true), BooleanCondition.ConditionType.AND);
			bc.AddCondition(new MockCondition(true), BooleanCondition.ConditionType.AND);

			bool isMet = bc.IsMet(null);

			Assert.IsTrue(isMet);
		}

		[TestMethod]
		public void MultipleANDFail()
		{
			BooleanCondition bc = new BooleanCondition();
			bc.AddCondition(new MockCondition(false), BooleanCondition.ConditionType.AND);
			bc.AddCondition(new MockCondition(true), BooleanCondition.ConditionType.AND);

			bool isMet = bc.IsMet(null);

			Assert.IsFalse(isMet);
		}

		[TestMethod]
		public void MultipleANDFail2()
		{
			BooleanCondition bc = new BooleanCondition();
			bc.AddCondition(new MockCondition(true), BooleanCondition.ConditionType.AND);
			bc.AddCondition(new MockCondition(false), BooleanCondition.ConditionType.AND);

			bool isMet = bc.IsMet(null);

			Assert.IsFalse(isMet);
		}

		[TestMethod]
		public void LastORPass()
		{
			BooleanCondition bc = new BooleanCondition();
			bc.AddCondition(new MockCondition(false), BooleanCondition.ConditionType.AND);
			bc.AddCondition(new MockCondition(false), BooleanCondition.ConditionType.AND);
			bc.AddCondition(new MockCondition(true), BooleanCondition.ConditionType.OR);

			bool isMet = bc.IsMet(null);

			Assert.IsTrue(isMet);
		}

		[TestMethod]
		public void MiddleORFail()
		{
			BooleanCondition bc = new BooleanCondition();
			bc.AddCondition(new MockCondition(false), BooleanCondition.ConditionType.AND);
			bc.AddCondition(new MockCondition(true), BooleanCondition.ConditionType.OR);
			bc.AddCondition(new MockCondition(false), BooleanCondition.ConditionType.AND);

			bool isMet = bc.IsMet(null);

			Assert.IsFalse(isMet);
		}

		[TestMethod]
		public void Not()
		{
			BooleanCondition bc = new BooleanCondition();
			bc.AddCondition(new MockCondition(false), BooleanCondition.ConditionType.AND | BooleanCondition.ConditionType.NOT);

			bool isMet = bc.IsMet(null);

			Assert.IsTrue(isMet);
		}
	}
}
