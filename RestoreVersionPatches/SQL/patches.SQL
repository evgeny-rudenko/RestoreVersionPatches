select 
version =( select VALUE from sys_option where code='DB_VERSION'),
date = (select VALUE from sys_option where code='DB_VERSION_DATE')
union 
select version, date from patches
order by date