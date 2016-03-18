$(function() {
	$('#open').click(function() {

		//	var ss ='223344';
		//	console.log(ss.substr(0,2));
		$.ajax({
			type: "post",
			url: "get_data.php",
			success: function(data) {
				var json = JSON.parse(data);
				var ticks = new Array(); //横坐标值 
				var data1Value = new Array(); //数据  
				var data2Value = new Array(); //数据  
				var data3Value = new Array(); //数据  
				var html = '';
				for (var i = 0; i < json.length; i++) {
					ticks.push(json[i].id_device);
					data1Value.push(parseInt(json[i].data1));
					data2Value.push(parseInt(json[i].data2));
					data3Value.push(parseInt(json[i].data3));

					html += '<div class="content"><h2>' + json[i].id_device + '<em>' + json[i].time + '</em></h2><p>' + json[i].type_device + json[i].data1 + json[i].data2 + +json[i].data3 + '</p></div>';

				}
				alert(data1Value);
				$('#data').html(html);
				$('#container').highcharts({
					title: {
						text: 'Monthly Average Temperature',
						x: -20 //center
					},
					subtitle: {
						text: 'Source: WorldClimate.com',
						x: -20
					},
					xAxis: {
						//						categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
						categories: ticks
					},
					yAxis: {
						title: {
							text: 'Temperature (°C)'
						},
						plotLines: [{
							value: 0,
							width: 1,
							color: '#808080'
						}]
					},
					tooltip: {
						valueSuffix: '°C'
					},
					legend: {
						layout: 'vertical',
						align: 'right',
						verticalAlign: 'middle',
						borderWidth: 0
					},
					series: [{
							name: 'data1',
							//						data: [7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9, 9.6]
							data: data1Value
						}, {
							name: 'data2',
							//						data: [-0.2, 0.8, 5.7, 11.3, 17.0, 22.0, 24.8, 24.1, 20.1, 14.1, 8.6, 2.5]
							data: data2Value
						}, {
							name: 'data3',
							//						data: [-0.9, 0.6, 3.5, 8.4, 13.5, 17.0, 18.6, 17.9, 14.3, 9.0, 3.9, 1.0]
							data: data3Value
						}
						//					, {
						//						name: 'London',
						//						data: [data1Value[0],data2Value[0],data3Value[0]]
						//							//						data: [3.9, 4.2, 5.7, 8.5, 11.9, 15.2, 17.0, 16.6, 14.2, 10.3, 6.6, 4.8]
						//					}
					]

				});



			},
			async: true,
		});
	})

});






//ajax({
//	method: 'post',
//	url: 'get_data.php',
//	data: {},
//	success: function(text) {
//		var json = JSON.parse(text);
//		var html = '';
//		for (var i = 0; i < json.length; i++) {
//			html += '<div class="content"><h2>' + json[i].title + '<em>' + json[i].date + '</em></h2><p>' + json[i].content + '</p></div>';
//		}
//		$('#index').html(html);
//		for (var i = 0; i < json.length; i++) {
//			$('#index .content').eq(i).animate({
//				attr: 'o',
//				target: 100,
//				t: 30,
//				step: 10
//
//			});
//		}
//
//	},
//	async: true
//});