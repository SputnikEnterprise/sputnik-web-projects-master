Die IBANKernel.dll ist eine C/C++ DLL von Six Payment Services. Die DLL erlaub es eine BC-Number und Kontonummer in eine IBAN Nummer zu konvertieren.
Das IBAN Tool für Windows (und  der IBANKernel.dll) kann auf dieser Seite geladen werden http://www.six-interbank-clearing.com/de/home/standardization/iban/iban-tool.html.
Die Funktionen der DLL sind mittels P/Invoke gemappt (siehe IBANUtil/IBANToolDllDeclarations). Die Funtionen werden dan von der IBANUtil/IBANEncoder Klasse eingesetzt.

Achtung!

Die IBANKernel.dll hat intern ein Ablaufdatum eingebaut, danach funktioniert die DLL nicht mehr.
Nächstes Ablaufdatum ist der 30.06.2015. Die DLL muss dann nochmals von der oben angeführten Seite geladen werden und im Projekt ersetzt werden.
