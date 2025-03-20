# Неделя 5: Домашнее задание

## Перед тем как начать
- Как подготовить окружение [см. тут](./docs/01-prepare-environment.md)
- Как накатить миграции на базу данных [см. тут](./docs/02-data-migrations.md)
- **Самое важное!** - полное описание базы данных, схему и описание полей можно найти [тут](./docs/03-db-description.md)

## Основные требования
- решением каждого задания является **один** SQL-запрос
- не допускается менять схему или сами данные, если это явно не указано в задании
- поля в выборках должны иметь псевдоним (alias), указанный в задании
- решение необходимо привести в блоке каждой задачи ВМЕСТО комментария "ЗДЕСЬ ДОЛЖНО БЫТЬ РЕШЕНИЕ" (прямо в текущем readme.md файле)
- метки времени должны быть приведены в формат _dd.MM.yyyy HH:mm:ss_ (время в БД и выборках в UTC)

## Прочие пожелания
- всем будет удобно, если вы будете придерживаться единого стиля форматирования SQL-команд, как в [этом примере](./docs/04-sql-guidelines.md)

## Задание 1: 100 заданий с наименьшим временем выполнения
Время, затраченное на выполнение задания - это период времени, прошедший с момента перехода задания в статус "В работе" и до перехода в статус "Выполнено".
Нужно вывести 100 заданий с самым быстрым временем выполнения. 
Полученный список заданий должен быть отсортирован от заданий с наименьшим временем выполнения к заданиям с наибольшим временем выполнения.

Замечания:
- Невыполненные задания (не дошедшие до статуса "Выполнено") не учитываются.
- Когда исполнитель берет задание в работу, оно переходит в статус "В работе" (InProgress) и находится там до завершения работы. После чего переходит в статус "Выполнено" (Done).
  В любой момент времени задание может быть безвозвратно отменено - в этом случае оно перейдет в статус "Отменено" (Canceled).
- Нет разницы, выполняется задание или подзадание.
- Выборка должна включать задания за все время.

Выборка должна содержать следующий набор полей:
- номер задания (task_number)
- заголовок задания (task_title)
- название статуса задания (status_name)
- email автора задания (author_email)
- email текущего исполнителя (assignee_email)
- дата и время создания задания (created_at)
- дата и время первого перехода в статус "В работе" (in_progress_at)
- дата и время выполнения задания (completed_at)
- количество дней, часов, минут и секунд, которые задание находилось в работе - в формате "dd HH:mm:ss" (work_duration)

### Решение
```sql
select t.number as task_number
     , t.title as task_title
	   , ts.name as status_name
	   , u.email as author_email
	   , u2.email as assignee_email
	   , t.created_at as created_at
     , tl.at as in_progress_at
     , t.completed_at as completed_at
     , (tl2.at - tl.at) as work_duration
from task_logs tl
join task_logs tl2 on tl2.task_id = tl.task_id and tl2.status = '4' /* Done */
join tasks t on t.id = tl.task_id
join task_statuses ts on ts.id = t.status
join users u on u.id = t.created_by_user_id
join users u2 on u2.id = t.assigned_to_user_id
where tl.status = '3' /* InProgress */
and tl.task_id in (
	select tl.task_id
	from task_logs tl
	where tl.status in ('3', '4')
	group by tl.task_id 
	having count(distinct tl.status) = 2
)
order by work_duration asc
limit 100
```

## Задание 2: выборка для проверки вложенности
Задания могут быть простыми и составными. Составное задание содержит в себе дочерние - так получается иерархия заданий.
Глубина иерархии ограничена Н-уровнями, поэтому перед добавлением подзадачи к текущей задаче нужно понять, может ли пользователь добавить задачу уровнем ниже текущего или нет. Для этого нужно написать выборку для метода проверки перед добавлением подзадания, которая бы вернула уровень вложенности указанного задания и полный путь до него от родительского задания.

Замечания:
- id проверяемого задания передаем в SQL как параметр _:parent_task_id_
- если задание _Е_ находится на 5м уровне, то путь должен быть "_//A/B/C/D/E_".

Выборка должна содержать:
- только 1 строку
- поле "Уровень задания" (level) - уровень указанного в параметре задания
- поле "Путь" (path)

### Решение
```sql
with recursive tasks_tree
  as (select t.id as task_id
           , t.parent_task_id as parent_task_id
           , 1 as level
           , '//' || t.id::text as path
      from tasks t
      where t.id = :parent_task_id
      union all
      select t.id as task_id
           , t.parent_task_id as parent_task_id
           , tt.level + 1 as level
           , '//' || t.id::text || tt.path
      from tasks t
      join tasks_tree tt on tt.parent_task_id = t.id)
select tt.level, tt. path
from tasks_tree tt
order by level desc
limit 1
```

## Задание 3: самые активные пользователи
Для того, чтобы понимать, насколько активно пользователи используют сервис управления заданиями - необходимо собирать топ 100 активных пользователей по количеству действий, сделанных в системе. Под действием подразумевается любое изменение/создание задачи, а также оставленные комментарии к задаче. Активным пользователем будем считать пользователя, у которого отсутствует блокировка.
Полученный список должен быть отсортирован в порядке убывания по количеству действий, совершенных пользователем. 

Замечания:
- в случае равного количества действий у пользователя, приоритетнее тот, у которого id пользователя меньше

Выборка должна содержать:
- id пользователя (user_id)
- email пользователя (email)
- количество действий, совершенных пользователем в системе (total_events)

### Решение
```sql
select res.id as user_id
     , u2.email as email
     , sum(res.count_active) AS total_events
from (
	select u.id as id
       , count(*) as count_active
	from tasks t 
	join users u on u.id = t.created_by_user_id
	where u.blocked_at is null
	group by u.id
	union all
	select u.id as id
       , count(*) as count_active
	from task_logs tl 
	join users u on u.id = tl.user_id
	where u.blocked_at is not null
	group by u.id
	union all
	select u.id as id
       , count(*) as count_active
	from task_comments tc 
	join users u on u.id = tc.author_user_id
	where u.blocked_at is not null
	group by u.id
) as res
join users u2 on u2.id = res.id
group by res.id, u2.email 
order by total_events desc, res.id asc
limit 100
```

## Дополнительное задание: получить переписку по заданию в формате "вопрос-ответ"
По заданию могут быть оставлены комментарии от Автора и Исполнителя. Автор задания - это пользователь, создавший задание, Исполнитель - это актуальный пользователь, назначенный на задание.
У каждого комментария есть поле _author_user_id_ - это идентификатор пользователя, который оставил комментарий. Если этот идентификатор совпадает с идентификатором Автора задания, то сообщение должно отобразиться **в левой колонке**, следующее за ним сообщение-ответ Исполнителя (если _author_user_id_ равен id исполнителя) должно отобразиться **на той же строчке**, что и сообщение Автора, но **в правой колонке**. Считаем, что Автор задания задает Вопросы, а Исполнитель дает на них Ответы.
Выборка должна включать "беседы" по 5 самым новым заданиям и быть отсортирована по порядку отправки комментариев в рамках задания. 

Замечания:
- Актуальный исполнитель - это пользователь на момент выборки указан в поле assiged_to_user_id.
- Если вопроса или ответа нет, в соответствующем поле должен быть NULL.
- Считаем, что все сообщения были оставлены именно текущим исполнителем (без учета возможных переназначений).
- Если комментарий был оставлен не Автором и не Исполнителем, игнорируем его.

Выборка должна содержать следующий набор полей:
- номер задания (task_number)
- email автора задания (author_email)
- email АКТУАЛЬНОГО исполнителя (assignee_email)
- вопрос (question)
- ответ (answer)
- метка времени, когда был задан вопрос (asked_at)
- метка времени, когда был дан ответ (answered_at)

<details>
  <summary>Пример</summary>

Переписка по заданию №1 между author@tt.ru и assgnee@tt.ru:
- 01.01.2023 08:00:00 (автор) "вопрос 1"
- 01.01.2023 09:00:00 (исполнитель) "ответ 1"
- 01.01.2023 09:15:00 (исполнитель) "ответ 2"
- 01.01.2023 09:30:00  (автор) "вопрос 2"

Ожидаемый результат выполнения SQL-запроса:

| task_number | author_email    | assignee_email | question  | answer  | asked_at             | answered_at          |
|-------------|-----------------|----------------|-----------|---------|----------------------|----------------------|
| 1           | author@tt.ru    | assgnee@tt.ru  | вопрос 1  | ответ 1 | 01.01.2023 08:00:00  | 01.01.2023 09:00:00  |
| 1           | author@tt.ru    | assgnee@tt.ru  | вопрос 1  | ответ 2 | 01.01.2023 08:00:00  | 01.01.2023 09:15:00  |
| 1           | author@tt.ru    | assgnee@tt.ru  | вопрос 2  |         | 01.01.2023 09:30:00  |                      |

</details>


### Решение
```sql
with recent_tasks as (
    select id, number
         , created_by_user_id
         , assigned_to_user_id
    from tasks
    order by created_at desc
    limit 5
),
comments as (
    select tc.task_id,
         , tc.author_user_id
         , u.email as author_email
         , tc.message
         , tc.at
         , t.created_by_user_id as task_author_id
         , t.assigned_to_user_id as task_assignee_id
         , au.email as task_author_email
         , asu.email as task_assignee_email,
    case when tc.author_user_id = t.created_by_user_id then 'question' end as is_question,
    case when tc.author_user_id = t.assigned_to_user_id then 'answer' end as is_answer
    from task_comments tc
    join recent_tasks t on tc.task_id = t.id
    join users u on tc.author_user_id = u.id
    join users au on t.created_by_user_id = au.id
    join users asu on t.assigned_to_user_id = asu.id
    where tc.author_user_id in (t.created_by_user_id, t.assigned_to_user_id)
),
matched_comments as (
    select task_id
         , task_author_email as author_email
         , task_assignee_email as assignee_email
         , message as question
         , at as asked_at
         , lag(message) over (partition by task_id order by at) as potential_answer
         , lag(at) over ( partition by task_id order by at) as potential_answered_at
    from comments
    where is_question = 'question'
),
result as (
    select m.task_id
         , m.author_email
         , m.assignee_email
         , m.question
         , m.asked_at
         , case when c.is_answer = 'answer' then m.potential_answer end as answer
         , case when c.is_answer = 'answer' then m.potential_answered_at end as answered_at
    from matched_comments m
    left join comments c 
        on m.task_id = c.task_id 
        and c.message = m.potential_answer 
        and c.at = m.potential_answered_at
)
select rt.number as task_number
     , fm.author_email  
     , m.assignee_email
     , fm.question
     , fm.answer
     , fm.asked_at
     , fm.answered_at
from recent_tasks rt
left join result fm on rt.id = fm.task_id
order by rt.number, coalesce(fm.asked_at, now())
```
### Дедлайны сдачи и проверки задания:
- 22 марта 23:59 (сдача) / 25 марта, 23:59 (проверка)
