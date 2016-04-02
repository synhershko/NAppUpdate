# NAppUpdate

[![NuGet version](https://badge.fury.io/nu/NAppUpdate.Framework.svg)](http://badge.fury.io/nu/NAppUpdate.Framework)

An application auto-update framework for .NET

## Contributions

Contributions are more than welcome.

How to contribute:

1. Create an issue explaining the problem you solved
2. Create a pull request that links to this issue
3. Follow the coding standard

## Coding standard

1. Use tabs for indentation
2. Always end the document with a new line
3. Avoid commiting lines that are irrelevant for the change (e.g. random white space changes)

An [.editorconfig](http://editorconfig.org/) file is available in the project, which makes it easy to follow the coding standard by installing the [EditorConfig](https://visualstudiogallery.msdn.microsoft.com/c8bccfe2-650c-4b42-bc5c-845e21f96328) and [Format document on Save](https://visualstudiogallery.msdn.microsoft.com/3ea1c920-69c4-441f-9979-ccc2752dac56) extensions in Visual Studio.

## How to pack NuGet

Run the following command in the NAppUpdate.Framework directory:

    nuget pack -Prop Configuration=Release

## Other notes

User and developer discussion group is at http://groups.google.com/group/nappupdate

Docs and about the philosophy behind NAppUpdate:
http://www.code972.com/blog/2012/06/the-philosophy-behind-nappupdate/

This project is partially based on work done by Lee Treveil,
in http://github.com/leetreveil/.NET-Auto-Update.

This software is licensed under the Apache License, Version 2.0
(the "License"); you may not use the files in this distribution
except in compliance with the License. You may obtain a copy of
the License at http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing,
software distributed under the License is distributed on an
"AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
either express or implied. See the License for the specific
language governing permissions and limitations under the License.

Copyright (c) 2010-2015 Itamar Syn-Hershko
