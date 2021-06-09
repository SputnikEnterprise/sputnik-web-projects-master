makecert.exe -r -pe -n "CN=mycockpitWebServiceCertificate" -sr LocalMachine -sv mycockpitWebService.pvk mycockpitWebService.cer -sky exchange

cert2spc mycockpitWebService.cer mycockpitWebService.spc

pvkimprt -pfx mycockpitWebService.spc mycockpitWebService.pvk