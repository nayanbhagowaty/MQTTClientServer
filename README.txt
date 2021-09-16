For SSL/TLS with Self signed certificates

Check Branch WithSSL


------------------------------------------------------------------------
1. Create CA key
openssl genrsa -des3 -out c:\cert\ca.key 2048

2. Create CA certificate
openssl req -new -x509 -days 1826 -key c:\cert\ca.key -out c:\cert\ca.crt
Output:
-----------------------------------------------------------------------
Country Name (2 letter code) [AU]:se
State or Province Name (full name) [Some-State]:got
Locality Name (eg, city) []:got
Organization Name (eg, company) [Internet Widgits Pty Ltd]:mycomp
Organizational Unit Name (eg, section) []:it
Common Name (e.g. server FQDN or YOUR name) []: Note: Common Name should be the server that client is connected to
Email Address []:
---------------------------------------------------------------------------
3. Create client key
openssl genrsa -out c:\cert\client.key 2048

4. Create client certificate request
openssl req -new -out c:\cert\client.csr -key c:\cert\client.key
-------------------------------------------------------------------------
Country Name (2 letter code) [AU]:se
State or Province Name (full name) [Some-State]:sh
Locality Name (eg, city) []:sh
Organization Name (eg, company) [Internet Widgits Pty Ltd]: client comp
Organizational Unit Name (eg, section) []:it
Common Name (e.g. server FQDN or YOUR name) []:Note: Common Name should be the server that client is connected to
Email Address []:
----------------------------------------------------------------------------------

5. Create client certificate
openssl x509 -req -in c:\cert\client.csr -CA c:\cert\ca.crt -CAkey c:\cert\ca.key -CAcreateserial -out c:\cert\client.crt -days 360

6. Create PFX file from client key, Cert, and private key
openssl pkcs12 -export -out c:\cert\client.pfx -inkey c:\cert\client.key -in c:\cert\client.crt -certfile c:\cert\ca.crt
Note: add password if needed, which will be added later in code

7. Create server private key
openssl genrsa -out c:\cert\server.key 2048

8. Create server certificate request
openssl req -new -out c:\cert\server.csr -key c:\cert\server.key
--------------------------------------------------------------------------------------
Country Name (2 letter code) [AU]:se
State or Province Name (full name) [Some-State]:got
Locality Name (eg, city) []:got
Organization Name (eg, company) [Internet Widgits Pty Ltd]: my comp
Organizational Unit Name (eg, section) []:it
Common Name (e.g. server FQDN or YOUR name) []:Note: Common Name should be the server
Email Address []:
-------------------------------------------------------------------------------------------

9. Create server certificate
openssl x509 -req -in c:\cert\server.csr -CA c:\cert\ca.crt -CAkey c:\cert\ca.key -CAcreateserial -out c:\cert\server.crt -days 360

Settings in Mosquitto.conf 
port 8883
# capath
cafile =  c:\cert\ca.crt

# Path to the PEM encoded server certificate.
certfile = c:\cert\server.crt

# Path to the PEM encoded keyfile.
keyfile = c:\cert\server.key
 
