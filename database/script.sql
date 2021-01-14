---------------- Schemas start-----------------------


create schema if not exists users;



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

    ------------------------ Tables End -------------------------


    ------------------ Types start ----------------------------

    drop type if exists users.followstatus;
    create type users.followstatus as enum('Pending', 'Accepted');

    ------------------ Types start ----------------------------
