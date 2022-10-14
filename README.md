# Mail Servis API

Projeyi onion mimarisi ile yazdım. Aynı zamanda CQRS patterni kullandım.

Mail gönderimini api aracılığı ile yaptığınız anda gönderim yapılmıyor. Öncelikle rabbitmq ile sıraya koyuluyor ve consumer bunu sırası geldiğinde alarak mail gönderme işini yapıyor.

Proje de başlıca öğrendiğim konuları kullanmaya çalıştım.
Rabbitmq, onion architecture, CQRS, JWT Authentication...
