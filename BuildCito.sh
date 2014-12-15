rm Html/*.js

mono CitoAssets.exe Data Assets.ci.cs
mono CitoAssets.exe Data2 Assets2.ci.cs
mono cito.exe -D CITO -D JS -D JSTA -l js-ta -o Html/Mario.js $(ls Mario/*.ci.cs) $(ls Mario/Scripts/*.ci.cs) $(ls Mario/Systems/*.ci.cs) Assets.ci.cs
mono cito.exe -D CITO -D JS -D JSTA -l js-ta -o Html/Assets2.js Assets2.ci.cs
