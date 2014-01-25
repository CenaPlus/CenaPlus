CREATE TABLE `contests` (
    id int not null auto_increment,
    title varchar(50) not null,
    description mediumtext not null,
    start_time datetime not null,
    end_time datetime not null,
    `type` tinyint not null,
    join_password binary(20) not null,
    manage_password binary(20) not null,
    primary key (id)
)  default charset=utf8;

CREATE TABLE `problems` (
    id int not null auto_increment,
    title varchar(50) not null,
    content mediumtext not null,
    contest_id int not null,
    primary key (id),
    foreign key (contest_id)
        references contests (id) on delete cascade
)  default charset=utf8;