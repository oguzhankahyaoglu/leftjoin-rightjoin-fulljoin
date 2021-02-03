# joinextensions-iqueryable
Very simple Left/Right/FullOuterJoin extension methods for both IQueryable and IEnumerable types

In linq, if you need a left outer join, you have to use GroupJoin/Selectmany/DefaultIfEmpty blocks. There is another package in nuget containing similar extensions for only IEnumerable, which results in performance loss for database queries. For this need, I developed IQueryable versions. 
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
- Left Join for IEnumerable via ON statement condition: other overloads uses lambdas for **LEFT=> key** and **RIGTH => key** syntax, this overload uses **(LEFT, RIGHT) => boolean** syntax

# Usage

| Tables   |      Are      |  Cool |
|-----------------|-----------------------|------|
| LEFT JOIN       | LeftJoinExt()           | UserRoleRepository.GetAll().LeftJoinExt(roles, ur => ur.RoleId, role => role.Id) |
| LEFT EXCL JOIN  | LeftExcludingJoin()     | UserRoleRepository.GetAll().LeftExcludingJoinExt(roles, ur => ur.RoleId, role => role.Id) |
| RIGHT JOIN      | RightJoinExt()          | UserRoleRepository.GetAll().LeftExcludingJoinExt(roles, ur => ur.RoleId, role => role.Id) |
| RIGHT EXCL JOIN | RightExcludingJoinExt() | UserRoleRepository.GetAll().LeftExcludingJoinExt(roles, ur => ur.RoleId, role => role.Id) |
| INNER JOIN      | InnerJoinExt()          | UserRoleRepository.GetAll().InnerJoinExt(roles, ur => ur.RoleId, role => role.Id) |
| FULL JOIN       | FullOuterJoinExt()      | UserRoleRepository.GetAll().FullOuterJoinExt(roles, ur => ur.RoleId, role => role.Id) |
| FULL EXCL JOIN  | FullOuterExcludingJoinExt() | UserRoleRepository.GetAll().FullOuterExcludingJoinExt(roles, ur => ur.RoleId, role => role.Id) |
