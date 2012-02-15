regedit /s MaxScriptStatements.reg

java -jar selenium-server-standalone-2.0.0.jar -role webdriver -hub http://10.202.135.18:4444/grid/register -port 5557 -browser browserName="internet explorer",platform=WINDOWS,version=IE9