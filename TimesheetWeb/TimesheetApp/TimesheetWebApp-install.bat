echo Timesheet Web App installation.
echo Allows non-admistrators to listen for http on port 41978
echo Run as administrator.
echo %USERNAME%
netsh http add urlacl url=http://localhost:41978/ user=%USERNAME%