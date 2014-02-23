drop table if exists `users`;
drop table if exists `email_forbiddens`;

CREATE TABLE `users` (
    id int not null auto_increment,
    `name` varchar(20) not null,
    `password` binary(20) not null,
    nick_name varchar(50) not null,
	real_name varchar(50) default null,
	identification_number varchar(50) default null,
	email varchar(100) not null,
	gravatar varchar(100) default null,
    `role` tinyint not null,
    primary key (id),
    unique index (`name`)
)  default charset=utf8;

CREATE TABLE `email_forbiddens` (
    id int not null auto_increment,
    `address` varchar(50) not null,
    primary key (id),
    unique index (`address`)
)  default charset=utf8;