# joinextensions-iqueryable
Very simple Left/Right/FullOuterJoin extension methods for both IQueryable and IEnumerable types

In linq, if you need a left outer join, you have to use GroupJoin/Selectmany/DefaultIfEmpty blocks. There is another package in nuget containing similar extensions for only IEnumerable, which results in performance loss for database queries. For this need, I developed IQueryable versions. 
