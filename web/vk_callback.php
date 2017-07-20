<?php 
header("Content-Type: application/json; encoding=utf-8; charset=utf-8"); 
header("Cache-Control: no-cache");

require("common.php");

$secret_key = 'Yzr95iIXkAEUkBToDgV2'; // Защищенный ключ приложения 

$input = $_POST; 

// Проверка подписи 
$sig = $input['sig']; 
unset($input['sig']); 
ksort($input); 
$str = ''; 
foreach ($input as $k => $v) { 
  $str .= $k.'='.$v; 
} 

if ($sig != md5($str.$secret_key)) { 
  $response['error'] = array( 
    'error_code' => 10, 
    'error_msg' => 'Несовпадение вычисленной и переданной подписи запроса.', 
    'critical' => true 
  ); 
} else { 
  // Подпись правильная 
  switch ($input['notification_type']) { 
    case 'get_item': 
   case 'get_item_test': 
      // Получение информации о товаре в тестовом режиме 
      $item = $input['item']; 
      if ($item == 'tries100') { 
        $response['response'] = array( 
          'item_id' => 101, 
          'title' => "100 новых попыток", 
          'photo_url' => 'trafficstorm.concretemixergames.com/webgl2/img/tries100.png', 
          'price' => 7
        ); 
      } elseif ($item == 'tries1000') { 
        $response['response'] = array( 
          'item_id' => 102, 
          'title' => "1000 новых попыток", 
          'photo_url' => 'trafficstorm.concretemixergames.com/webgl2/img/tries1000.png', 
          'price' => 35 
        ); 
      } else { 
        $response['error'] = array( 
          'error_code' => 20, 
          'error_msg' => "Товара $item не существует", 
          'critical' => true 
        ); 
      } 
      break; 

case 'order_status_change_test': 
case 'order_status_change': 
      // Изменение статуса заказа 
      if ($input['status'] == 'chargeable') { 
        $order_id = intval($input['order_id']); 
        $user = $input['receiver_id'];
        $item = $input['item']; 
        $dt = $input['date'];

        $db = @mysqli_connect("localhost", $db_user, $db_pass, $db_name);

        $sql = "INSERT INTO orders (user,order_id,item,dt) VALUES ($user,$order_id,'$item',$dt)";
         if (mysqli_query($db, $sql)) {
            $app_order_id = mysqli_insert_id($db);

            if ($item=='tries100')
               $sql = "UPDATE `tries` SET tries=tries+100,last_try=NOW() WHERE user=$user";
            elseif ($item=='tries1000')
               $sql = "UPDATE `tries` SET tries=tries+1000,last_try=NOW() WHERE user=$user";
            else {
             $response['error'] = array( 
               'error_code' => 103, 
               'error_msg' => 'item error', 
               'critical' => true 
             ); 
            }
            if (mysqli_query($db, $sql)) {
              $response['response'] = array( 
                'order_id' => $order_id, 
                'app_order_id' => $app_order_id, 
              ); 
             }
           else {
             $response['error'] = array( 
               'error_code' => 102, 
               'error_msg' => 'DB error', 
               'critical' => true 
             ); 

           }
         }
         else {
             $response['error'] = array( 
               'error_code' => 101, 
               'error_msg' => 'DB error', 
               'critical' => true 
             ); 
         }
      } else { 
        $response['error'] = array( 
          'error_code' => 100, 
          'error_msg' => 'Передано непонятно что вместо chargeable.', 
          'critical' => true 
        ); 
      } 
      break; 
  } 
} 

echo json_encode($response, JSON_UNESCAPED_UNICODE); 
?> 