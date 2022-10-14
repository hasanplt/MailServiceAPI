# Mail Servis API

Projeyi onion mimarisi ile yazdım. Aynı zamanda CQRS patterni kullandım.

Mail gönderimini api aracılığı ile yaptığınız anda gönderim yapılmıyor. Öncelikle rabbitmq ile sıraya koyuluyor ve consumer bunu sırası geldiğinde alarak mail gönderme işini yapıyor.

Proje de başlıca öğrendiğim konuları kullanmaya çalıştım.
Rabbitmq, onion architecture, CQRS, JWT Authentication...

Projenin dosyalaması biraz saçma olma sebebi benim projeleri oluştururken dosya yapısını belirtmeyi unutmuş olmamdan kaynaklı.
Aslında projenin src klasörü içerisinde bir de Apps klasörü olmalı ve içinde:
IdentityService.Api
HasanPolatCom.WebApi
HasanPolatCom.Console
projeleri bulunmalıydı. Ancak bu projeler klasör yapısı içerisinde biraz dağıtık bir şekilde bulundu.
