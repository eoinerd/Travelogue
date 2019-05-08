// Write your Javascript code.
(function () {

    configureContent();
    addComment();
    createSubPost();
    submitSubPost();
    editSubPost();
    submitEditSubPost();
    scrollToCategorySubPost();
    scrollToTarget();

    $(".carousel-item").first().addClass("active");

    $("#sidebarToggle").on("click", function () {
        $("#sidebar").toggleClass("hide-sidebar");
        $(this).children('.fa').toggleClass('fa-chevron-circle-up fa-chevron-circle-down');
    });

})();

// Functions
function configureContent() {
    $("video").prop("volume", 0.0);
    var imgBackground = "url(" + $("#backgroundImg").attr("src") + ")";
    $(".landingContainer").css("background-image", imgBackground);
}

function addComment() {
    $("#addCommentBtn").on("click", function () {
        var comment = {
            Text: $("#userComment").val(),
            PostId: $("#hdnPostId").val()
        };

        $.ajax({
            url: "/Posts/AddComment",
            type: "POST",
            cache: false,
            data: comment
        }).done(function (result) {
            $("#userComment").val("");
            $("#commentsOnPageLoad").hide();
            $("#comments").html(result);
        }).fail(function (xhr) {
            console.log("error: " + xhr.status + " - " + xhr.statusText + " - " + xhr.responseText);
        });
    });
}

function createSubPost() {
    $("#createSubPost").click(function () {
        $(".sub-posts-create").css("display", "block");

        $('#summerNoteCreateSubPost').summernote({
            height: 200,
            popover: {
                image: [],
                link: [],
                air: []
            }
        });

        $('html, body').animate({
            scrollTop: $("#createSubPostForm").offset().top - 100
        }, 1000);
    });
}

function submitSubPost() {
    $("body").on("click", "#submitSubPost", function () {
        var subPost = {
            Category: $("#category").val(),
            SubPostText: $("#summerNoteCreateSubPost").val(),
            PostId: $("#hdnPostId").val(),
            Id: $("#hdnSubPostId").val()
        };

        $.ajax({
            url: "/Posts/AddSubPost",
            type: "POST",
            cache: false,
            data: subPost
        }).done(function (result) {
            $(".sub-posts").css("display", "none");
            $("#initialSubPosts").remove();
            $("#subPostEdit").css("display", "none");
            $("#subPosts").html(result);
        }).fail(function (xhr) {
            console.log("error: " + xhr.status + " - " + xhr.statusText + " - " + xhr.responseText);
        });
    });
}

function submitEditSubPost() {
    $("body").on("click", "#submitEditSubPost", function () {
        var parentId = $(this).parent().attr('id');
        var subPostText = "#summernote" + parentId;
        var subPost = {
            Category: $("#categoryEdit" + parentId).val(),
            SubPostText: $(subPostText).val(),
            PostId: $("#hdnPostIdEdit").val(),
            Id: parentId
        };

        $.ajax({
            url: "/Posts/AddSubPost",
            type: "POST",
            cache: false,
            data: subPost
        }).done(function (result) {
            var outerDiv = $(result);
            $("#initialSubPosts").remove();
            $("#subPosts").html(result);
        }).fail(function (xhr) {
            console.log("error: " + xhr.status + " - " + xhr.statusText + " - " + xhr.responseText);
        });
    });
}

function scrollToCategorySubPost()
{
    $(".sub-post-category").on("click", function () {
        var catName = $(this).text();
        $('html, body').animate({
            scrollTop: $("#" + catName).offset().top - 100
        }, 1000);
    });
}

function editSubPost() {
    $("body").on("click", ".editSubPostBtn", function () {
        var subPostId = this.id;

        $('#summernote' + subPostId).summernote({
            height: 200,
            popover: {
                image: [],
                link: [],
                air: []
            }
        });

        var outerDiv = $(this).parent().parent().parent();
        outerDiv.hide();
        $("#showEdit" + subPostId).show();
    });
}

function scrollToTarget() {
    // Select all links with hashes
    $('a[href*="#"]')
        // Remove links that don't actually link to anything
        .not('[href="#"]')
        .not('[href="#0"]')
        .click(function (event) {
            // On-page links
            if (
                location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '')
                    &&
                    location.hostname == this.hostname
            ) {
                // Figure out element to scroll to
                var target = $(this.hash);
                target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
                // Does a scroll target exist?
                if (target.length) {
                    // Only prevent default if animation is actually gonna happen
                    event.preventDefault();
                    $('html, body').animate({
                        scrollTop: target.offset().top
                    }, 1000, function () {
                        // Callback after animation
                        // Must change focus!
                        var $target = $(target);
                        $target.focus();
                        if ($target.is(":focus")) { // Checking if the target was focused
                            return false;
                        } else {
                            $target.attr('tabindex', '-1'); // Adding tabindex for elements not focusable
                            $target.focus(); // Set focus again
                        };
                    });
                }
            }
        });
}