#!/bin/bash

# Start the OWASP ZAP security tool
/System/Volumes/Data/Applications/OWASP\ ZAP.app/Contents/Java/zap.sh -daemon -port 8080

# Wait for ZAP to start
sleep 10

# Start the security scan using ZAP's API
curl -s -X POST http://localhost:8080/JSON/spider/action/scan/ -d "url=https://www.thecocktaildb.com/api/json/v2/1/search.php?s=margarita"

# Wait for the security scan to complete
sleep 60

# Generate the HTML security report
curl -s -X GET http://localhost:8080/OTHER/core/other/htmlreport/ -o security_report.html

# Shutdown ZAP
curl -s -X POST http://localhost:8080/JSON/core/action/shutdown/

echo "OWASP ZAP security scan completed."
