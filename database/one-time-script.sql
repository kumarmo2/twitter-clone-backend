create database twitter;
create schema if not exists users;
create schema if not exists events;
create schema if not exists notifications;
create schema if not exists tweets;


    ------------------ Types start ----------------------------

    -- This should be run after script.sql
    create type users.followstatus as enum('Pending', 'Accepted');

    ------------------ Types start ----------------------------
