<?php
require 'config.php';
$query = mysql_query("SELECT id_device,type_device,data1,data2,data3,time FROM c_data ORDER BY time DESC LIMIT 0,3") or die('SQL错误');




$json='';
while (!!$row=mysql_fetch_array($query, MYSQL_ASSOC)) {
	$json.=json_encode($row).',';
};
echo '['.substr($json, 0,strlen($json)-1).']';


mysql_close();
?>