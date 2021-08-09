@echo off

SET /A initialPort=4835
SET /A endPort=4839

:label
	start "" "EmotionServer.exe" "-listen_port=%initialPort%"
	IF NOT %initialPort% EQU %endPort% (set /A initialPort=%initialPort%+1 & goto:label)

pause