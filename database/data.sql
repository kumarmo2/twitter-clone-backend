-- as the name suggests, will only be run after the all the tables, schemas has been setup.

insert into notifications.notificationtypes (id, name)
values
(1, 'FollowRequestCreate'),
(2, 'FollowRequestAccept');