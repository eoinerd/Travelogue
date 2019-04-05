// Write your Javascript code.
(function () {

    //var ele = $("#userName");
    //ele.text("Eoin Denneo");

    //var main = $("#main");

    //main.on("mouseenter", function () {
    //    main.style.backgroundColor = "#888";
    //});
    //main.on("mouseleave", function () {
    //    main.style.backgroundColor = "";
    //});

    //var menuItems = $("ul.menu li a");

    //menuItems.on("click", function () {
    //    var me = $(this);
    //    alert(me.text());
    //});

    var $sidebarAndWrapper = $("#sidebar,#wrapper");
    var $toggleButton = $("#sidebarToggle");
    var $icon = $("#sidebarToggle i.fa");
    var $wrapper = $("#wrapper");

    $("#sidebarToggle").on("click", function () {
        //$sidebarAndWrapper.toggleClass("hide-sidebar");
        $("#sidebar").toggleClass("hide-sidebar");
        //if($sidebarAndWrapper.hasClass("hide-sidebar")){
        //    $icon.removeClass("fa-chevron-circle-left");
        //    $icon.addClass("fa-chevron-circle-right");
        //    $toggleButton.addClass("collapse-toggle");
        //    $toggleButton.removeClass("expand-toggle");
        //} else {
        //    $icon.removeClass("fa-chevron-circle-right");
        //    $icon.addClass("fa-chevron-circle-left");
        //    $toggleButton.addClass("expand-toggle");
        //    $toggleButton.removeClass("collapse-toggle");
        //}
    });

    var ImageRotator = function (options) {
        // Set main component properties
        this.width = options.width + 'px';
        this.height = options.height + 'px';
        this.interval = options.interval || 1000;
        this.transition = options.transition || '0.4s linear';
        this.imagesToLoad = options.images;
        // Create sub-view-models for each image
        this.imageViewModels = [];
        options.images.forEach(function (image) {
            var imageViewModel = {
                url: image,
                active: ko.observable(false)
            };
            this.imageViewModels.push(imageViewModel);
        }.bind(this));
        // Show "Loading" panel until first image is loaded
        this.showLoading = ko.observable(true);
        // define empty array to control displayed images
        this.images = ko.observableArray();
        // start loading of first image
        this.loadFirstImage();
    };


    ImageRotator.prototype.nextImage = function () {
        this.images.valueWillMutate();
        var top = this.images.pop();
        var ind = this.imageViewModels.indexOf(top);
        var nextInd = (ind + 2) % this.imageViewModels.length;
        var next = this.imageViewModels[nextInd];
        this.images.unshift(next);
        top.active(false);
        this.images()[1].active(true);
        this.images.valueHasMutated();
    };

    ImageRotator.prototype.onCtrlClick = function (imageVM) {
        if (imageVM === this.images()[1]) {
            return;
        }
        clearInterval(this.switchInterval);
        var top = this.images.pop();
        this.images()[0] = imageVM;
        var ind = this.imageViewModels.indexOf(imageVM);
        var nextInd = (ind + 1) % this.imageViewModels.length;
        var next = this.imageViewModels[nextInd];
        this.images.unshift(next);
        top.active(false);
        imageVM.active(true);
        this.images.valueHasMutated();
        this.switchInterval = setInterval(this.nextImage.bind(this), this.interval);
    };

    ImageRotator.prototype.removeDecorator = function (elem) {
        var removeElement = function () {
            elem.parentNode.removeChild(elem)
        };
        elem.addEventListener('webkitTransitionEnd', removeElement);
        elem.addEventListener('TransitionEnd', removeElement);
        elem.style.left = this.width;
        elem.style['-webkit-transition'] = elem.style['transition'] = this.transition;
    };

    ImageRotator.prototype.loadFirstImage = function () {
        var first = this.imagesToLoad.shift();
        this.loadImage(first, function () {
            // Make first image "active" - currently displayed
            this.imageViewModels[0].active(true);
            this.images.push(this.imageViewModels[0]);
            this.images.unshift(this.imageViewModels[1]);
            this.showLoading(false);
            this.imagesToLoad.shift();
            // set image switching interval
            this.switchInterval = setInterval(this.nextImage.bind(this), this.interval);
            this.loadRemainingImages();
        }.bind(this));
    };

    ImageRotator.prototype.loadRemainingImages = function () {
        this.imagesToLoad.forEach(function (image) {
            this.loadImage(image);
        }.bind(this));
    };

    ImageRotator.prototype.loadImage = function (imgUrl, loadCallback) {
        var imageEl = document.createElement('img');
        if (loadCallback) {
            imageEl.addEventListener('load', loadCallback);
        }
        imageEl.src = imgUrl;
    };

    var images = ['http://cdn-image.travelandleisure.com/sites/default/files/styles/1600x1000/public/brussels-evening-belg0316.jpg?itok=thl3zxby',
        'http://cdn-image.travelandleisure.com/sites/default/files/styles/1600x1000/public/1476113065/barcelona-spain-waterfront-SPDEAL1016.jpg?itok=CKzwLZaO',
        'http://images.trvl-media.com/media/content/shared/images/travelguides/Burlington-601763-smallTabletRetina.jpg',
        'https://upload.wikimedia.org/wikipedia/commons/b/bd/Pula_Aerial_View.jpg',
        'https://image.jimcdn.com/app/cms/image/transf/none/path/sa6549607c78f5c11/image/id5715922f9bbddee/version/1472727543/prague.jpg'];
        var rotator = new ImageRotator({
            images: images,
            width: 640,
            height: 426,
            transition: '3.4s linear',
            interval: 2000
        });
        ko.applyBindings(rotator, document.body);

   
})();


