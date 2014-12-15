CitoAssets Data Assets.ci.cs
CitoAssets Data2 Assets2.ci.cs

del /q /s CitoOutput

mkdir CitoOutput
mkdir CitoOutput\C
mkdir CitoOutput\Java
mkdir CitoOutput\Cs
mkdir CitoOutput\JsTa

copy CitoPlatform\Cs\* CitoOutput\Cs\*
copy CitoPlatform\Java\* CitoOutput\Java\*
copy CitoPlatform\JsTa\* CitoOutput\JsTa\*
copy CitoPlatform\C\* CitoOutput\C\*

setlocal enabledelayedexpansion enableextensions
set LIST=
for %%x in (Mario\*.ci.cs) do set LIST=!LIST! %%x
for %%x in (Mario\Scripts\*.ci.cs) do set LIST=!LIST! %%x
for %%x in (Mario\Systems\*.ci.cs) do set LIST=!LIST! %%x
set LIST=%LIST:~1%
echo %LIST%

IF NOT "%1"=="fast" cito -D CITO -D C -l c -o CitoOutput\C\Mario.c %LIST% Assets.ci.cs
IF NOT "%1"=="fast" cito -D CITO -D JAVA -l java -o CitoOutput\Java\Mario.java -n mario.lib  %LIST% Assets.ci.cs
IF NOT "%1"=="fast" cito -D CITO -D CS -l cs -o CitoOutput\Cs\Mario.cs %LIST% Assets.ci.cs
cito -D CITO -D JS -D JSTA -l js-ta -o CitoOutput\JsTa\Mario.js %LIST% Assets.ci.cs

copy CitoOutput\JsTa\Mario.js Html\Mario.js

cito -D CITO -D JS -D JSTA -l js-ta -o Html\Assets2.js Assets2.ci.cs