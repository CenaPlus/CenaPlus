drop table if exists `user_assigned_contests`;
drop table if exists `records`;
drop table if exists `problems`;
drop table if exists `contests`;
drop table if exists `users`;

CREATE TABLE `users` (
    id int not null auto_increment,
    `name` varchar(20) not null,
    `password` binary(20) not null,
    role tinyint not null,
    primary key (id),
    unique index (`name`)
)  default charset=utf8;

CREATE TABLE `contests` (
    id int not null auto_increment,
    title varchar(50) not null,
    description mediumtext not null,
    start_time datetime not null,
    end_time datetime not null,
    `type` tinyint not null,
    primary key (id),
    index (title)
)  default charset=utf8;

CREATE TABLE `problems` (
    id int not null auto_increment,
    title varchar(50) not null,
    content mediumtext not null,
    contest_id int not null,
    primary key (id),
    foreign key (contest_id)
        references contests (id)
        on delete cascade,
    index (title)
)  default charset=utf8;

CREATE TABLE `records` (
    id int not null auto_increment,
    user_id int not null,
    problem_id int not null,
    `status` tinyint not null,
    `language` tinyint not null,
    `code` mediumtext not null,
    submission_time datetime not null,
    time_usage int default null,
    memory_usage bigint default null,
    detail mediumtext default null,
    primary key (id),
    foreign key (user_id)
        references users (id)
        on delete cascade,
    foreign key (problem_id)
        references problems (id)
        on delete cascade,
    index (`status`)
)  default charset=utf8;

CREATE TABLE `user_assigned_contests` (
    user_id int not null,
    contest_id int not null,
    primary key (user_id , contest_id),
    foreign key (user_id)
        references users (id)
        on delete cascade,
    foreign key (contest_id)
        references contests (id)
        on delete cascade
)  default charset=utf8;