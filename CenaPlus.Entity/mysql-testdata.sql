INSERT INTO users (`name`,`password`,role) values ('GasaiYuno',unhex(sha1('GasaiYuno')),1);
INSERT INTO users (`name`,`password`,role) values ('onetwogoo',unhex(sha1('onetwogoo')),1);
INSERT INTO users (`name`,`password`,role) values ('user1',unhex(sha1('user1')),0);
INSERT INTO users (`name`,`password`,role) values ('user2',unhex(sha1('user2')),0);
INSERT INTO users (`name`,`password`,role) values ('user3',unhex(sha1('user3')),0);
INSERT INTO users (`name`,`password`,role) values ('user4',unhex(sha1('user4')),0);
INSERT INTO users (`name`,`password`,role) values ('user5',unhex(sha1('user5')),0);
INSERT INTO users (`name`,`password`,role) values ('user6',unhex(sha1('user6')),0);

INSERT INTO contests (title, description, start_time, end_time, `type`) values ('OI', 'Sample OI', addtime(now(),'0:1'),addtime(now(),'0:3'),0);
INSERT INTO contests (title, description, start_time, end_time, `type`) values ('ACM', 'Sample ACM', addtime(now(),'0:2'),addtime(now(),'0:4'),1);
INSERT INTO contests (title, description, start_time, end_time, `type`) values ('CF', 'Sample CF', addtime(now(),'0:3'),addtime(now(),'0:5'),2);
INSERT INTO contests (title, description, start_time, end_time, `type`) values ('TC', 'Sample TC', addtime(now(),'0:4'),addtime(now(),'0:6'),3);

INSERT INTO problems (title, content, contest_id) values ('OI A+B','a+b',1); 
INSERT INTO problems (title, content, contest_id) values ('OI C+D','c+d',1);
INSERT INTO problems (title, content, contest_id) values ('ACM A+B','a+b',2);
INSERT INTO problems (title, content, contest_id) values ('ACM C+D','c+d',2);
INSERT INTO problems (title, content, contest_id) values ('CF A+B','a+b',3);
INSERT INTO problems (title, content, contest_id) values ('CF C+D','c+d',3);
INSERT INTO problems (title, content, contest_id) values ('TC A+B','a+b',4);
INSERT INTO problems (title, content, contest_id) values ('TC C+D','c+d',4);

INSERT INTO records (user_id, problem_id, `status`,`language`,`code`,submission_time) values (1,1,0,0,'#include<con>',now());

INSERT INTO user_assigned_contests (user_id,contest_id) values(3,1);
INSERT INTO user_assigned_contests (user_id,contest_id) values(3,2);
INSERT INTO user_assigned_contests (user_id,contest_id) values(4,2);
INSERT INTO user_assigned_contests (user_id,contest_id) values(4,3);
