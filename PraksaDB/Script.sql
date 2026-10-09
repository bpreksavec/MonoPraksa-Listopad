create table "Workouts" (
    id uuid PRIMARY KEY,
    sport VARCHAR(50),
    distance DOUBLE PRECISION,
    duration INTEGER
);

create table athletes (
	id uuid primary key,
	name VARCHAR(30),
	surname VARCHAR(50),
	age INTEGER,
	height integer,
	weight integer
);

alter table athletes
alter column id
set default gen_random_uuid();

alter table "Workouts"
alter column id
set default gen_random_uuid();

select * from "Workouts";
select * from athletes;

alter table "Workouts"
drop column athlete_id;

alter table "Workouts"
add column athlete_id uuid;

alter table "Workouts"
add column avg_heartrate INTEGER;

alter table "Workouts"
add constraint fk_workout_athlete
foreign key (athlete_id)
references athletes(id);

alter table athletes 
add column country VARCHAR(30);

insert into athletes(name, surname, age, height, weight, country)
values 
	 ('Ivano','Balic',46,192,89,'Croatia'),
	 ('Mate','Licanin',27,179,69,'Croatia'),
	 ('Hans','Gechenhunker',17,201,88,'Germany'),
	 ('Milos','Kerkez',22,181,84,'Hungary'),
	 ('Janez','Kek',33,177,78,'Slovenia');

insert into "Workouts"(sport, distance, duration, avg_heartrate)
values
	('running', 5, 25.20, 163),
	('swimming', 0.3, 13.5, 172),
	('biking', 32, 95, 162),
	('running', 10, 55.1, 177),
	('running', 21.1, 106, 183),
	('swimming', 0.6, 19, 158),
	('running', 18, 99, 179),
	('biking', 92, 225, 167),
	('biking', 22, 64, 169),
	('swimming', 0.5, 12, 166);
	
select name, surname, age from athletes 
where age<33
order by name;

update athletes
set name = 'Milo', age=29
where name='Mile';

select name, surname 
from athletes
inner join "Workouts"
on athletes.id = "Workouts".id;

select sport
from "Workouts"
left join athletes on "Workouts".id = athletes.id
order by athletes.name;

select country, count(*)
from athletes
group by country;

drop table "Workouts";
drop table athletes;