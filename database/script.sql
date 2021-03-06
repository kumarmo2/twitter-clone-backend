---------------- Schemas start-----------------------

---------------- Schemas ends---------------------


------------------------ Tables Start -------------------------

    create table if not exists users.users
(
 id bigint not null,
 firstname varchar(50) not null,
 lastname varchar(50) not null,
 displayname varchar(200) not null,
 handle varchar(30) not null,
 email varchar(80) not null,
 hashedpassword text not null,
 isdeactivated bool not null default false,
 createdat timestamp not null,
 modifiedat timestamp not null default now(),
 constraint ix_users_handle unique(handle),
 constraint pk_users_id primary key(id)
 );

alter table users.users drop constraint if exists ix_users_email;

alter table users.users
    add constraint ix_users_email unique(email);

    create table if not exists users.follows
(
 id bigint not null,
 followerid bigint not null,
 followeeid bigint not null,
 status users.followstatus not null,
 constraint fk_follows_users_followerid foreign key(followerid) references users.users(id),
 constraint fk_follows_users_followeeid foreign key(followeeid) references users.users(id),
 constraint pk_follows_id primary key(id)
 );

 alter table users.follows drop constraint  if exists ix_follows_followerid_followeeid;

 alter table users.follows
add constraint ix_follows_followerid_followeeid unique(followerid, followeeid);

create table if not exists events.userqueues
(
	id bigint not null,
	userid bigint not null,
	queuename varchar(50) not null,
	constraint pk_userqueues_id primary key(id)
); 


alter table events.userqueues drop constraint if exists fk_userqueues_users;

alter table events.userqueues drop constraint if exists ix_userqueues_userid_queuename;

alter table events.userqueues
 add constraint fk_userqueues_users foreign key(userid) references users.users(id),
 add constraint ix_userqueues_userid_queuename unique(userid, queuename);


 create table if not exists notifications.notificationtypes
 (
     id bigint not null,
     name varchar(50) not null,
     constraint pk_notificationtypes primary key(id),
     constraint ix_notificationtypes_name unique(name)
 );

 create table if not exists notifications.notifications
 (
     id bigint not null,
     userid bigint not null,
     content text not null,
     url text,
     type bigint not null,
     isseen boolean default(false),
     constraint fk_notifications_notificationtypes_type foreign key(type) references notifications.notificationtypes(id),
     constraint fk_notifications_users_userid foreign key(userid) references users.users(id),
     constraint pk_notifications_id primary key(id)
 );


 create table if not exists tweets.tweets
 (
     id bigint not null,
     authorid bigint not null,
     createdat timestamp not null default(now()),
     modifiedat timestamp not null default(now()),
     content varchar(140) not null,
     parenttweetid bigint null,
     quotedtweetid bigint null,
     retweetedtweetid bigint null,
     constraint fk_tweets_tweets_retweetedtweetid foreign key(retweetedtweetid) references tweets.tweets(id),
     constraint fk_tweets_tweets_quotedtweetid foreign key(quotedtweetid) references tweets.tweets(id),
     constraint fk_tweets_tweets_parenttweetid foreign key(parenttweetid) references tweets.tweets(id),
     constraint fk_tweets_users_authorid foreign key(authorid) references users.users(id),
     constraint pk_tweets_id primary key(id)
 );

    ------------------------ Tables End -------------------------