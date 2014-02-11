INSERT INTO users (`name`,nick_name,`password`,role) values ('yuno','Gasai Yuno',unhex(sha1('GasaiYuno')),2);
INSERT INTO users (`name`,nick_name,`password`,role) values ('onetwogoo','Mr.Phone',unhex(sha1('onetwogoo')),2);
INSERT INTO users (`name`,nick_name,`password`,role) values ('user1','u1',unhex(sha1('user1')),1);
INSERT INTO users (`name`,nick_name,`password`,role) values ('user2','u2',unhex(sha1('user2')),1);
INSERT INTO users (`name`,nick_name,`password`,role) values ('user3','u3',unhex(sha1('user3')),1);
INSERT INTO users (`name`,nick_name,`password`,role) values ('user4','u4',unhex(sha1('user4')),1);
INSERT INTO users (`name`,nick_name,`password`,role) values ('user5','u5',unhex(sha1('user5')),1);
INSERT INTO users (`name`,nick_name,`password`,role) values ('user6','u6',unhex(sha1('user6')),0);

INSERT INTO contests (title, description, start_time, end_time, `type`,printing_enabled) values ('Sample OI', 'Sample OI', addtime(now(),'0:1'),addtime(now(),'0:3'),0,false);
INSERT INTO contests (title, description, start_time, end_time, `type`,printing_enabled) values ('Sample ACM', 'Sample ACM', addtime(now(),'0:2'),addtime(now(),'0:4'),1,true);
INSERT INTO contests (title, description, start_time, end_time, `type`,printing_enabled) values ('Sample CF', 'Sample CF', addtime(now(),'0:3'),addtime(now(),'0:5'),2,true);
INSERT INTO contests (title, description, start_time,rest_time,hack_start_time, end_time, `type`,printing_enabled) values ('Sample TC', 'Sample TC', addtime(now(),'0:4'),addtime(now(),'0:5'),addtime(now(),'0:6'),addtime(now(),'0:7'),3,false);

INSERT INTO problems (title, content, contest_id,score,time_limit,memory_limit,forbidden_languages) values ('OI A+B','a+b',1,1,1000,256*1024*1024,'C|CXX11'); 
INSERT INTO problems (title, content, contest_id,score,time_limit,memory_limit,forbidden_languages) values ('OI C+D','c+d',1,2,1000,256*1024*1024,'');
INSERT INTO problems (title, content, contest_id,score,time_limit,memory_limit,forbidden_languages) values ('ACM A+B','a+b',2,1,1000,256*1024*1024,'');
INSERT INTO problems (title, content, contest_id,score,time_limit,memory_limit,forbidden_languages) values ('ACM C+D','c+d',2,2,1000,256*1024*1024,'');
INSERT INTO problems (title, content, contest_id,score,time_limit,memory_limit,forbidden_languages) values ('CF A+B','a+b',3,1,1000,256*1024*1024,'');
INSERT INTO problems (title, content, contest_id,score,time_limit,memory_limit,forbidden_languages) values ('CF C+D','c+d',3,2,1000,256*1024*1024,'');
INSERT INTO problems (title, content, contest_id,score,time_limit,memory_limit,forbidden_languages) values ('TC A+B','a+b',4,1,1000,256*1024*1024,'');
INSERT INTO problems (title, content, contest_id,score,time_limit,memory_limit,forbidden_languages) values ('TC C+D','c+d',4,2,1000,256*1024*1024,'');

INSERT INTO test_cases (problem_id,`type`,input_hash,input,output) values (1,0,unhex(md5('1 1')),'1 1','2');
INSERT INTO test_cases (problem_id,`type`,input_hash,input,output) values (5,0,unhex(md5('1 2')),'1 2','3');
INSERT INTO test_cases (problem_id,`type`,input_hash,input,output) values (5,1,unhex(md5('2 2')),'2 2','4');

INSERT INTO records (user_id, problem_id, `status`,`language`,`code`,submission_time) values (1,1,0,0,'#include<con>',now());
INSERT INTO records (user_id, problem_id, `status`,`language`,`code`,submission_time,time_usage,memory_usage,detail) values (2,5,4,1,'#include <iostream>\nint main(){\nreturn 0;\n}',now(),998,1024*1024*3,'Detail....');

INSERT INTO hacks (record_id,hacker_id,`status`,data_or_datamaker,datamaker_language,detail) values (2,1,0,'#include<con>',0,'Details...');

INSERT INTO questions (asker_id,contest_id,`time`,`status`,description,answer) values (3,2,now(),0,'How to ask a question?',null);
INSERT INTO questions (asker_id,contest_id,`time`,`status`,description,answer) values (4,2,addtime(now(),'0:1'),1,'Why are these problems so easy?','Because the competitors are so weak.');
INSERT INTO questions (asker_id,contest_id,`time`,`status`,description,answer) values (5,2,addtime(now(),'0:2'),2,'How to solve this knapsack problem?','By means of a DP algorithm.');
INSERT INTO questions (asker_id,contest_id,`time`,`status`,description,answer) values (6,2,addtime(now(),'0:3'),3,'Fxxk your mother!','Keep silence or I will ban you out!');

INSERT INTO print_requests (user_id, contest_id, copies, content, `time`,`status`) values (1,2,1,'Test printing', now(),3);
INSERT INTO print_requests (user_id, contest_id, copies, content, `time`,`status`) values (2,3,2,'I wanna print!', addtime(now(),'0:1'),1);

INSERT INTO configs(`key`,`value`) values('circular','The Cena+ server is in debug mode.');