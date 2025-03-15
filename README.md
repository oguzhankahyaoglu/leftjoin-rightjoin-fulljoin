# leftjoin-rightjoin-fulljoin-outerjoin

Very simple Left/Right/FullOuterJoin extension methods for both IQueryable and IEnumerable types

In linq, if you need a left outer join, you have to use GroupJoin/Selectmany/DefaultIfEmpty blocks. There is another
package in nuget containing similar extensions for only IEnumerable, which results in performance loss for database
queries. For this need, I developed IQueryable versions.

# Installation

[![nuget](https://img.shields.io/nuget/v/leftjoin-rightjoin-fulljoin-outerjoin.svg)](https://www.nuget.org/packages/leftjoin-rightjoin-fulljoin-outerjoin/)

Functionalities:

- Left Join for IQueryables: Translated into database queries
- Left Join for IEnumerables: In memory joining
- Left Excluding Join for IQueryable/IEnumerables: Taking only Left side for matching items
- Right Join for IQueryable
- Right Join for IEnumerable
- Right Excluding Join for IQueryable/IEnumerables
- Full Outer Join for IQueryable
- Full Outer Join for IEnumerable
- Left Join for IEnumerable via ON statement condition: other overloads uses lambdas for **LEFT=> key** and **RIGTH =>
  key** syntax, this overload uses **(LEFT, RIGHT) => boolean** syntax

# Usage

### For IQueryable:

| **JOIN TYPE**   | **METHOD NAME**             | **Example**                                                                            | **RETURN TYPE**                     |
|-----------------|-----------------------------|----------------------------------------------------------------------------------------|-------------------------------------|
| LEFT JOIN       | LeftJoinExt()               | repository.GetAll().LeftJoinExt(roles, ur => ur.RoleId, role => role.Id)               | IQueryable<JoinItem<TLeft, TRight>> | 
| LEFT EXCL JOIN  | LeftExcludingJoin()         | repository.GetAll().LeftExcludingJoinExt(roles, ur => ur.RoleId, role => role.Id)      | IQueryable<JoinItem<TLeft, TRight>> | 
| RIGHT JOIN      | RightJoinExt()              | repository.GetAll().LeftExcludingJoinExt(roles, ur => ur.RoleId, role => role.Id)      | IQueryable<JoinItem<TLeft, TRight>> | 
| RIGHT EXCL JOIN | RightExcludingJoinExt()     | repository.GetAll().LeftExcludingJoinExt(roles, ur => ur.RoleId, role => role.Id)      | IQueryable<JoinItem<TLeft, TRight>> | 
| INNER JOIN      | InnerJoinExt()              | repository.GetAll().InnerJoinExt(roles, ur => ur.RoleId, role => role.Id)              | IQueryable<JoinItem<TLeft, TRight>> | 
| FULL JOIN       | FullOuterJoinExt()          | repository.GetAll().FullOuterJoinExt(roles, ur => ur.RoleId, role => role.Id)          | IQueryable<JoinItem<TLeft, TRight>> | 
| FULL EXCL JOIN  | FullOuterExcludingJoinExt() | repository.GetAll().FullOuterExcludingJoinExt(roles, ur => ur.RoleId, role => role.Id) | IQueryable<JoinItem<TLeft, TRight>> | 

### For IEnumerable:

| **JOIN TYPE**   | **METHOD NAME**                   | **Example**                                                                                                             | **RETURN TYPE**                      |
|-----------------|-----------------------------------|-------------------------------------------------------------------------------------------------------------------------|--------------------------------------|
| LEFT JOIN       | LeftJoinExtEnumerable()           | repository.GetAll().LeftJoinExtEnumerable(roles, ur => ur.RoleId, role => role.Id)                                      | IEnumerable<JoinItem<TLeft, TRight>> | 
|                 | LeftJoinExtEnumerable()           | repository.GetAll().LeftJoinExtEnumerable(roles, ur => ur.RoleId, role => role.Id, (left, right)=> new {...})           | IEnumerable< TResult >               |       
| LEFT EXCL JOIN  | LeftExcludingJoinExtEnumerable()  | repository.GetAll().LeftExcludingJoinExtEnumerable(roles, ur => ur.RoleId, role => role.Id, (left, right)=> new {...})  | IEnumerable< TResult >               | 
| RIGHT JOIN      | RightJoinExtEnumerable()          | repository.GetAll().RightJoinExtEnumerable(roles, ur => ur.RoleId, role => role.Id, (left, right)=> new {...})          | IEnumerable< TResult >               | 
| RIGHT EXCL JOIN | RightExcludingJoinExtEnumerable() | repository.GetAll().RightExcludingJoinExtEnumerable(roles, ur => ur.RoleId, role => role.Id, (left, right)=> new {...}) | IEnumerable< TResult >               | 
| INNER JOIN      | InnerJoinExtEnumerable()          | repository.GetAll().InnerJoinExtEnumerable(roles, ur => ur.RoleId, role => role.Id)                                     | IEnumerable<JoinItem<TLeft, TRight>> | 
|                 |                                   | repository.GetAll().InnerJoinExtEnumerable(roles, ur => ur.RoleId, role => role.Id, (left, right)=> new {...})          | IEnumerable< TResult >               |          
| FULL JOIN       | FullOuterJoinExtEnumerable()      | repository.GetAll().FullOuterJoinExtEnumerable(roles, ur => ur.RoleId, role => role.Id, (left, right)=> new {...})      | IEnumerable< TResult >               | 
