create database warsztat;

use warsztat;

create table Meble(
	Id int auto_increment primary key,
    Nazwa varchar(100) not null,
    Opis varchar(255)
);

create table Narzedzia(
	Id int auto_increment primary key,
    Nazwa varchar(100) not null,
    Opis varchar(255),
    Mebel_Id int,
    
    constraint FK_Narzedzia_Meble foreign key (Mebel_Id) references Meble(ID) on delete set null on update cascade
);

insert into Meble(Nazwa, Opis) values ('Stół', 'Duży stół warsztatowy');

insert into Narzedzia(Nazwa, Opis, Mebel_id) values ('Młotek', 'Standardowy młotek', 1);
insert into Narzedzia(Nazwa, Opis, Mebel_id) values ('Śrubokręt', 'Płaski śrubokręt', 1);

drop database warsztat;

create user 'warsztat_webapp'@'localhost' identified by 'HasloAplikacji';

grant all privileges on warsztat.* to 'warsztat_webapp'@'localhost';

flush privileges;