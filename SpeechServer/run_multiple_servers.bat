@echo off

SET /A initialPort=4830
SET /A endPort=4830

:label
	start "" "SpeechServer.exe" "-listen_port=%initialPort%"
	IF NOT %initialPort% EQU %endPort% (set /A initialPort=%initialPort%+1 & goto:label)

pause