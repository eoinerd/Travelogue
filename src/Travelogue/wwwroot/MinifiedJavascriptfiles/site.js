!function(){$("#addCommentBtn").on("click",function(){var e={Text:$("#userComment").val(),PostId:$("#hdnPostId").val()};$.ajax({url:"/Posts/AddComment",type:"POST",cache:!1,data:e}).done(function(e){$("#userComment").val(""),$("#commentsOnPageLoad").hide(),$("#comments").html(e)}).fail(function(e){console.log("error: "+e.status+" - "+e.statusText+" - "+e.responseText)})}),$('a[href*="#"]').not('[href="#"]').not('[href="#0"]').click(function(e){if(location.pathname.replace(/^\//,"")==this.pathname.replace(/^\//,"")&&location.hostname==this.hostname){var t=$(this.hash);t=t.length?t:$("[name="+this.hash.slice(1)+"]"),t.length&&(e.preventDefault(),$("html, body").animate({scrollTop:t.offset().top},1e3,function(){var e=$(t);if(e.focus(),e.is(":focus"))return!1;e.attr("tabindex","-1"),e.focus()}))}}),$(".carousel-item").first().addClass("active");$("#sidebar,#wrapper"),$("#sidebarToggle"),$("#sidebarToggle i.fa"),$("#wrapper");$("#sidebarToggle").on("click",function(){$("#sidebar").toggleClass("hide-sidebar"),$(this).children(".fa").toggleClass("fa-chevron-circle-up fa-chevron-circle-down")})}();