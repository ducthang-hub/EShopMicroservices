# eshop.microservices

## Note
Create ssl certification
```
openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout server.key -out server.crt -subj "/CN=main-domain-server" -addext "subjectAltName=DNS:alternative-domain-1,DNS:alternative-domain-2"
openssl pkcs12 -export -out server.pfx -inkey server.key -in server.crt -passout pass:your-password
```
