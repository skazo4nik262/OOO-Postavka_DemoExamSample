select o.id, 
sum(oi.count*si.material_count*pl.price) as "total_price"
from orders o
join order_items oi on o.id = oi.order_id
join specifications s on oi.product_id = s.product_id
join specification_items si on s.id = si.specification_id
join price_list pl on si.material_id = pl.product_id
where o.id = 1
group by o.id;
