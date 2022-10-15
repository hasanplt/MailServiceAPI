# Mail Servis API

  Projeyi onion mimarisi ile yazdım. Aynı zamanda CQRS patterni kullandım.

  Mail gönderimini api aracılığı ile yaptığınız anda gönderim yapılmıyor. Öncelikle rabbitmq ile sıraya koyuluyor ve consumer bunu sırası geldiğinde alarak mail gönderme işini yapıyor.

  Proje de başlıca öğrendiğim konuları kullanmaya çalıştım.
Rabbitmq, onion architecture, CQRS, JWT Authentication...

  Projenin dosyalaması biraz saçma olma sebebi benim projeleri oluştururken dosya yapısını belirtmeyi unutmuş olmamdan kaynaklı.
  Aslında projenin src klasörü içerisinde bir de Apps klasörü olmalı ve içinde:
    IdentityService.Api ,
    HasanPolatCom.WebApi ,
    HasanPolatCom.Console ,
  projeleri bulunmalıydı. Ancak bu projeler klasör yapısı içerisinde biraz dağıtık bir şekilde bulundu.

## API Nasıl işliyor ?

* İlk yapmamız gereken işlem kayıt olmaktır. Kayıt olduktan sonra ise giriş yaparak JWT Tokenimizi almamız gerekmektedir.
* Tokenimizi aldıktan sonra mail gönderimlerimizi yapabiliriz.

## Peki Nasıl Kullanacağız ? 

##### Kayıt Olma

`https://localhost:7202/api/Auth/Register`

Parametreler:
* FirstName : String, Kullanıcının isimi.
* LastName : String, Kullanıcının soyisimi.
* City : String, Kullanıcının bulunduğu şehir.
* AccountPassword : String, Kullanıcının hesap şifresi.
* MailAddress : String, Kullanıcının var olan mail gönderiminde kullanılacak olan bir gmail adresi.
* MailPasssword : String, Kullanıcının gmail adresinin şifresi.

Cevap olarak kullanıcı value kısmında kullanıcı id'si dönecektir.

##### Giriş Yapma

`https://localhost:7202/api/Auth`

Parametreler:
* mail : String, Kullanıcının gmail adresi.
* password : String, Kullanıcının AccountPassword kısmına yazdığı şifresi.

Cevap olarak kullanıcı value kısmında JWT Tokeni dönecektir.
