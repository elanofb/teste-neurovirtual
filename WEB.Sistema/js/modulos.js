$.AdminLTE = {};

$.AdminLTE.options = {
    //Add slimscroll to navbar menus
    //This requires you to load the slimscroll plugin
    //in every page before app.js
    navbarMenuSlimscroll: true,
    navbarMenuSlimscrollWidth: "3px", //The width of the scroll bar
    navbarMenuHeight: "200px", //The height of the inner menu
    //General animation speed for JS animated elements such as box collapse/expand and
    //sidebar treeview slide up/down. This options accepts an integer as milliseconds,
    //'fast', 'normal', or 'slow'
    animationSpeed: 500,
    //Sidebar push menu toggle button selector
    sidebarToggleSelector: "[data-toggle='offcanvas']",
    //Activate sidebar push menu
    sidebarPushMenu: true,
    //Activate sidebar slimscroll if the fixed layout is set (requires SlimScroll Plugin)
    sidebarSlimScroll: true,
    //Enable sidebar expand on hover effect for sidebar mini
    //This option is forced to true if both the fixed layout and sidebar mini
    //are used together
    sidebarExpandOnHover: false,
    //BoxRefresh Plugin
    enableBoxRefresh: true,
    //Bootstrap.js tooltip
    enableBSToppltip: true,
    BSTooltipSelector: "[data-toggle='tooltip']",
    //Enable Fast Click. Fastclick.js creates a more
    //native touch experience with touch devices. If you
    //choose to enable the plugin, make sure you load the script
    //before AdminLTE's app.js
    enableFastclick: false,
    //Control Sidebar Options
    enableControlSidebar: true,
    controlSidebarOptions: {
        //Which button should trigger the open/close event
        toggleBtnSelector: "[data-toggle='control-sidebar']",
        //The sidebar selector
        selector: ".control-sidebar",
        //Enable slide over content
        slide: true
    },
    //Box Widget Plugin. Enable this plugin
    //to allow boxes to be collapsed and/or removed
    enableBoxWidget: true,
    //Box Widget plugin options
    boxWidgetOptions: {
        boxWidgetIcons: {
            //Collapse icon
            collapse: 'fa-minus',
            //Open icon
            open: 'fa-plus',
            //Remove icon
            remove: 'fa-times'
        },
        boxWidgetSelectors: {
            //Remove button selector
            remove: '[data-widget="remove"]',
            //Collapse button selector
            collapse: '[data-widget="collapse"]'
        }
    },
    //Direct Chat plugin options
    directChat: {
        //Enable direct chat by default
        enable: true,
        //The button to open and close the chat contacts pane
        contactToggleSelector: '[data-widget="chat-pane-toggle"]'
    },
    //Define the set of colors to use globally around the website
    colors: {
        lightBlue: "#3c8dbc",
        red: "#f56954",
        green: "#00a65a",
        aqua: "#00c0ef",
        yellow: "#f39c12",
        blue: "#0073b7",
        navy: "#001F3F",
        teal: "#39CCCC",
        olive: "#3D9970",
        lime: "#01FF70",
        orange: "#FF851B",
        fuchsia: "#F012BE",
        purple: "#8E24AA",
        maroon: "#D81B60",
        black: "#222222",
        gray: "#d2d6de"
    },
    //The standard screen sizes that bootstrap uses.
    //If you change these in the variables.less file, change
    //them here too.
    screenSizes: {
        xs: 480,
        sm: 768,
        md: 992,
        lg: 1200
    }
};

/* ------------------
 * - Implementation -
 * ------------------
 * The next block of code implements AdminLTE's
 * functions and plugins as specified by the
 * options above.
 */
$(function () {
    "use strict";

    //Fix for IE page transitions
    $("body").removeClass("hold-transition");

    //Extend options if external options exist
    if (typeof AdminLTEOptions !== "undefined") {
        $.extend(true, $.AdminLTE.options, AdminLTEOptions);
    }

    //Easy access to options
    var o = $.AdminLTE.options;

    //Set up the object
    _init();

    //Activate the layout maker
    $.AdminLTE.layout.activate();

    //Enable sidebar tree view controls
    $.AdminLTE.tree('.sidebar');

    //Enable control sidebar
    if (o.enableControlSidebar) {
        $.AdminLTE.controlSidebar.activate();
    }

    //Add slimscroll to navbar dropdown
    if (o.navbarMenuSlimscroll && typeof $.fn.slimscroll != 'undefined') {
        $(".navbar .menu").slimscroll({
            height: o.navbarMenuHeight,
            alwaysVisible: false,
            size: o.navbarMenuSlimscrollWidth
        }).css("width", "100%");
    }

    //Activate sidebar push menu
    if (o.sidebarPushMenu) {
        $.AdminLTE.pushMenu.activate(o.sidebarToggleSelector);
    }

    //Activate Bootstrap tooltip
    if (o.enableBSToppltip) {
        $('body').tooltip({
            selector: o.BSTooltipSelector,
            template: '<div class="tooltip" role="tooltip"><div class="tooltip-arrow"></div><div class="tooltip-inner tooltip-suggestion"></div></div>'
        });
    }

    //Activate box widget
    if (o.enableBoxWidget) {
        $.AdminLTE.boxWidget.activate();
    }

    //Activate fast click
    if (o.enableFastclick && typeof FastClick != 'undefined') {
        FastClick.attach(document.body);
    }

    //Activate direct chat widget
    if (o.directChat.enable) {
        $(document).on('click', o.directChat.contactToggleSelector, function () {
            var box = $(this).parents('.direct-chat').first();
            box.toggleClass('direct-chat-contacts-open');
        });
    }

    /*
     * INITIALIZE BUTTON TOGGLE
     * ------------------------
     */
    $('.btn-group[data-toggle="btn-toggle"]').each(function () {
        var group = $(this);
        $(this).find(".btn").on('click', function (e) {
            group.find(".btn.active").removeClass("active");
            $(this).addClass("active");
            e.preventDefault();
        });

    });
});

/* ----------------------------------
 * - Initialize the AdminLTE Object -
 * ----------------------------------
 * All AdminLTE functions are implemented below.
 */
function _init() {
    'use strict';
    /* Layout
     * ======
     * Fixes the layout height in case min-height fails.
     *
     * @type Object
     * @usage $.AdminLTE.layout.activate()
     *        $.AdminLTE.layout.fix()
     *        $.AdminLTE.layout.fixSidebar()
     */
    $.AdminLTE.layout = {
        activate: function () {
            var _this = this;
            _this.fix();
            _this.fixSidebar();
            $(window, ".wrapper").resize(function () {
                _this.fix();
                _this.fixSidebar();
            });
        },
        fix: function () {
            //Get window height and the wrapper height
            var neg = $('.main-header').outerHeight() + $('.main-footer').outerHeight();
            var window_height = $(window).height();
            var sidebar_height = $(".sidebar").height();
            //Set the min-height of the content and sidebar based on the
            //the height of the document.
            if ($("body").hasClass("fixed")) {
                $(".content-wrapper, .right-side").css('min-height', window_height - $('.main-footer').outerHeight());
            } else {
                var postSetWidth;
                if (window_height >= sidebar_height) {
                    $(".content-wrapper, .right-side").css('min-height', window_height - neg);
                    postSetWidth = window_height - neg;
                } else {
                    $(".content-wrapper, .right-side").css('min-height', sidebar_height);
                    postSetWidth = sidebar_height;
                }

                //Fix for the control sidebar height
                var controlSidebar = $($.AdminLTE.options.controlSidebarOptions.selector);
                if (typeof controlSidebar !== "undefined") {
                    if (controlSidebar.height() > postSetWidth)
                        $(".content-wrapper, .right-side").css('min-height', controlSidebar.height());
                }

            }
        },
        fixSidebar: function () {
            //Make sure the body tag has the .fixed class
            if (!$("body").hasClass("fixed")) {
                if (typeof $.fn.slimScroll != 'undefined') {
                    $(".sidebar").slimScroll({ destroy: true }).height("auto");
                }
                return;
            } else if (typeof $.fn.slimScroll == 'undefined' && window.console) {
                window.console.error("Error: the fixed layout requires the slimscroll plugin!");
            }
            //Enable slimscroll for fixed to
            if ($.AdminLTE.options.sidebarSlimScroll) {
                if (typeof $.fn.slimScroll != 'undefined') {
                    //Destroy if it exists
                    $(".sidebar").slimScroll({ destroy: true }).height("auto");
                    //Add slimscroll
                    $(".sidebar").slimscroll({
                        height: ($(window).height() - $(".main-header").height()) + "px",
                        color: "rgba(0,0,0,0.2)",
                        size: "3px"
                    });
                }
            }
        }
    };

    /* PushMenu()
     * ==========
     * Adds the push menu functionality to the sidebar.
     *
     * @type Function
     * @usage: $.AdminLTE.pushMenu("[data-toggle='offcanvas']")
     */
    $.AdminLTE.pushMenu = {
        activate: function (toggleBtn) {
            //Get the screen sizes
            var screenSizes = $.AdminLTE.options.screenSizes;

            //Enable sidebar toggle
            $(document).on('click', toggleBtn, function (e) {
                e.preventDefault();

                //Enable sidebar push menu
                if ($(window).width() > (screenSizes.sm - 1)) {
                    if ($("body").hasClass('sidebar-collapse')) {
                        $("body").removeClass('sidebar-collapse').trigger('expanded.pushMenu');
                    } else {
                        $("body").addClass('sidebar-collapse').trigger('collapsed.pushMenu');
                    }
                }
                    //Handle sidebar push menu for small screens
                else {
                    if ($("body").hasClass('sidebar-open')) {
                        $("body").removeClass('sidebar-open').removeClass('sidebar-collapse').trigger('collapsed.pushMenu');
                    } else {
                        $("body").addClass('sidebar-open').trigger('expanded.pushMenu');
                    }
                }
            });

            $(".content-wrapper").click(function () {
                //Enable hide menu when clicking on the content-wrapper on small screens
                if ($(window).width() <= (screenSizes.sm - 1) && $("body").hasClass("sidebar-open")) {
                    $("body").removeClass('sidebar-open');
                }
            });

            //Enable expand on hover for sidebar mini
            if ($.AdminLTE.options.sidebarExpandOnHover
              || ($('body').hasClass('fixed')
              && $('body').hasClass('sidebar-mini'))) {
                this.expandOnHover();
            }
        },
        expandOnHover: function () {
            var _this = this;
            var screenWidth = $.AdminLTE.options.screenSizes.sm - 1;
            //Expand sidebar on hover
            $('.main-sidebar').hover(function () {
                if ($('body').hasClass('sidebar-mini')
                  && $("body").hasClass('sidebar-collapse')
                  && $(window).width() > screenWidth) {
                    _this.expand();
                }
            }, function () {
                if ($('body').hasClass('sidebar-mini')
                  && $('body').hasClass('sidebar-expanded-on-hover')
                  && $(window).width() > screenWidth) {
                    _this.collapse();
                }
            });
        },
        expand: function () {
            $("body").removeClass('sidebar-collapse').addClass('sidebar-expanded-on-hover');
        },
        collapse: function () {
            if ($('body').hasClass('sidebar-expanded-on-hover')) {
                $('body').removeClass('sidebar-expanded-on-hover').addClass('sidebar-collapse');
            }
        }
    };

    /* Tree()
     * ======
     * Converts the sidebar into a multilevel
     * tree view menu.
     *
     * @type Function
     * @Usage: $.AdminLTE.tree('.sidebar')
     */
    $.AdminLTE.tree = function (menu) {
        
        var _this = this;
        _this.layout.fix();
        
        var animationSpeed = $.AdminLTE.options.animationSpeed;
        
        $(document).off('click', menu + ' li a')
          .on('click', menu + ' li a', function (e) {
              //Get the clicked link and the next element
              var $this = $(this);
              var checkElement = $this.next();

              //Check if the next element is a menu and is visible
              if ((checkElement.is('.treeview-menu')) && (checkElement.is(':visible')) && (!$('body').hasClass('sidebar-collapse'))) {
                  //Close the menu
                  checkElement.slideUp(animationSpeed, function () {
                      checkElement.removeClass('menu-open');
                      //Fix the layout in case the sidebar stretches over the height of the window
                      //_this.layout.fix();
                  });
                  checkElement.parent("li").removeClass("active");
              }
                  //If the menu is not visible
              else if ((checkElement.is('.treeview-menu')) && (!checkElement.is(':visible'))) {
                  //Get the parent menu
                  var parent = $this.parents('ul').first();
                  //Close all open menus within the parent
                  var ul = parent.find('ul:visible').slideUp(animationSpeed);
                  //Remove the menu-open class from the parent
                  ul.removeClass('menu-open');
                  //Get the parent li
                  var parent_li = $this.parent("li");

                  //Open the target menu and add the menu-open class
                  checkElement.slideDown(animationSpeed, function () {
                      //Add the class active to the parent li
                      checkElement.addClass('menu-open');
                      parent.find('li.active').removeClass('active');
                      parent_li.addClass('active');
                      //Fix the layout in case the sidebar stretches over the height of the window
                      _this.layout.fix();
                  });
              }
              //if this isn't a link, prevent the page from being redirected
              if (checkElement.is('.treeview-menu')) {
                  e.preventDefault();
              }
          });
    };

    /* ControlSidebar
     * ==============
     * Adds functionality to the right sidebar
     *
     * @type Object
     * @usage $.AdminLTE.controlSidebar.activate(options)
     */
    $.AdminLTE.controlSidebar = {
        //instantiate the object
        activate: function () {
            //Get the object
            var _this = this;
            //Update options
            var o = $.AdminLTE.options.controlSidebarOptions;
            //Get the sidebar
            var sidebar = $(o.selector);
            //The toggle button
            var btn = $(o.toggleBtnSelector);

            //Listen to the click event
            btn.on('click', function (e) {
                e.preventDefault();
                //If the sidebar is not open
                if (!sidebar.hasClass('control-sidebar-open')
                  && !$('body').hasClass('control-sidebar-open')) {
                    //Open the sidebar
                    _this.open(sidebar, o.slide);
                } else {
                    _this.close(sidebar, o.slide);
                }
            });

            //If the body has a boxed layout, fix the sidebar bg position
            var bg = $(".control-sidebar-bg");
            _this._fix(bg);

            //If the body has a fixed layout, make the control sidebar fixed
            if ($('body').hasClass('fixed')) {
                _this._fixForFixed(sidebar);
            } else {
                //If the content height is less than the sidebar's height, force max height
                if ($('.content-wrapper, .right-side').height() < sidebar.height()) {
                    _this._fixForContent(sidebar);
                }
            }
        },
        //Open the control sidebar
        open: function (sidebar, slide) {
            //Slide over content
            if (slide) {
                sidebar.addClass('control-sidebar-open');
            } else {
                //Push the content by adding the open class to the body instead
                //of the sidebar itself
                $('body').addClass('control-sidebar-open');
            }
        },
        //Close the control sidebar
        close: function (sidebar, slide) {
            if (slide) {
                sidebar.removeClass('control-sidebar-open');
            } else {
                $('body').removeClass('control-sidebar-open');
            }
        },
        _fix: function (sidebar) {
            var _this = this;
            if ($("body").hasClass('layout-boxed')) {
                sidebar.css('position', 'absolute');
                sidebar.height($(".wrapper").height());
                if (_this.hasBindedResize) {
                    return;
                }
                $(window).resize(function () {
                    _this._fix(sidebar);
                });
                _this.hasBindedResize = true;
            } else {
                sidebar.css({
                    'position': 'fixed',
                    'height': 'auto'
                });
            }
        },
        _fixForFixed: function (sidebar) {
            sidebar.css({
                'position': 'fixed',
                'max-height': '100%',
                'overflow': 'auto',
                'padding-bottom': '50px'
            });
        },
        _fixForContent: function (sidebar) {
            $(".content-wrapper, .right-side").css('min-height', sidebar.height());
        }
    };

    /* BoxWidget
     * =========
     * BoxWidget is a plugin to handle collapsing and
     * removing boxes from the screen.
     *
     * @type Object
     * @usage $.AdminLTE.boxWidget.activate()
     *        Set all your options in the main $.AdminLTE.options object
     */
    $.AdminLTE.boxWidget = {
        selectors: $.AdminLTE.options.boxWidgetOptions.boxWidgetSelectors,
        icons: $.AdminLTE.options.boxWidgetOptions.boxWidgetIcons,
        animationSpeed: $.AdminLTE.options.animationSpeed,
        activate: function (_box) {
            var _this = this;
            if (!_box) {
                _box = document; // activate all boxes per default
            }
            //Listen for collapse event triggers
            $(_box).on('click', _this.selectors.collapse, function (e) {
                e.preventDefault();
                _this.collapse($(this));
            });

            //Listen for remove event triggers
            $(_box).on('click', _this.selectors.remove, function (e) {
                e.preventDefault();
                _this.remove($(this));
            });
        },
        collapse: function (element) {
            var _this = this;
            //Find the box parent
            var box = element.parents(".box").first();
            //Find the body and the footer
            var box_content = box.find("> .box-body, > .box-footer, > form  >.box-body, > form > .box-footer");
            if (!box.hasClass("collapsed-box")) {
                //Convert minus into plus
                element.children(":first")
                  .removeClass(_this.icons.collapse)
                  .addClass(_this.icons.open);
                //Hide the content
                box_content.slideUp(_this.animationSpeed, function () {
                    box.addClass("collapsed-box");
                });
            } else {
                //Convert plus into minus
                element.children(":first")
                  .removeClass(_this.icons.open)
                  .addClass(_this.icons.collapse);
                //Show the content
                box_content.slideDown(_this.animationSpeed, function () {
                    box.removeClass("collapsed-box");
                });
            }
        },
        remove: function (element) {
            //Find the box parent
            var box = element.parents(".box").first();
            box.slideUp(this.animationSpeed);
        }
    };
}

/* ------------------
 * - Custom Plugins -
 * ------------------
 * All custom plugins are defined below.
 */

/*
 * BOX REFRESH BUTTON
 * ------------------
 * This is a custom plugin to use with the component BOX. It allows you to add
 * a refresh button to the box. It converts the box's state to a loading state.
 *
 * @type plugin
 * @usage $("#box-widget").boxRefresh( options );
 */
(function ($) {

    "use strict";

    $.fn.boxRefresh = function (options) {

        // Render options
        var settings = $.extend({
            //Refresh button selector
            trigger: ".refresh-btn",
            //File source to be loaded (e.g: ajax/src.php)
            source: "",
            //Callbacks
            onLoadStart: function (box) {
                return box;
            }, //Right after the button has been clicked
            onLoadDone: function (box) {
                return box;
            } //When the source has been loaded

        }, options);

        //The overlay
        var overlay = $('<div class="overlay"><div class="far fa-sync fa-spin"></div></div>');

        return this.each(function () {
            //if a source is specified
            if (settings.source === "") {
                if (window.console) {
                    window.console.log("Please specify a source first - boxRefresh()");
                }
                return;
            }
            //the box
            var box = $(this);
            //the button
            var rBtn = box.find(settings.trigger).first();

            //On trigger click
            rBtn.on('click', function (e) {
                e.preventDefault();
                //Add loading overlay
                start(box);

                //Perform ajax call
                box.find(".box-body").load(settings.source, function () {
                    done(box);
                });
            });
        });

        function start(box) {
            //Add overlay and loading img
            box.append(overlay);

            settings.onLoadStart.call(box);
        }

        function done(box) {
            //Remove overlay and loading img
            box.find(overlay).remove();

            settings.onLoadDone.call(box);
        }

    };

})(jQuery);

/*
 * EXPLICIT BOX CONTROLS
 * -----------------------
 * This is a custom plugin to use with the component BOX. It allows you to activate
 * a box inserted in the DOM after the app.js was loaded, toggle and remove box.
 *
 * @type plugin
 * @usage $("#box-widget").activateBox();
 * @usage $("#box-widget").toggleBox();
 * @usage $("#box-widget").removeBox();
 */
(function ($) {

    'use strict';

    $.fn.activateBox = function () {
        $.AdminLTE.boxWidget.activate(this);
    };

    $.fn.toggleBox = function () {
        var button = $($.AdminLTE.boxWidget.selectors.collapse, this);
        $.AdminLTE.boxWidget.collapse(button);
    };

    $.fn.removeBox = function () {
        var button = $($.AdminLTE.boxWidget.selectors.remove, this);
        $.AdminLTE.boxWidget.remove(button);
    };

})(jQuery);


var Vocabulary = {
    'project_title': '',

    'confirmUpdateStatus' : 'Confirma a altera&ccedil;&atilde;o do Status?',
    'confirmDelete': 'Tem certeza que deseja efetuar a exclus&atilde;o?<br /> Essa opera&ccedil;&atilde;o n&atilde;o poder&aacute; ser desfeita!',

    'invalid_cep': 'O CEP informado � inv&aacute;lido, tente novamente!',
    'unsuported_cep': 'O CEP informado n&atilde;o &eacute; atendido no momento!',

    'loading' : 'Carregando...',

    'select_state' : 'Por favor, selecione um estado.!',
    'select_newsletter' : 'Por favor, escolha ao menos uma not�cia para criar a newsletter.',
    'select' : 'Selecione...'
};
var ObjAjax = function () {

    this.vars = {
        method: 'post',
        returnType: 'json',
        callback: null,
        alertMessageDefault: null,
        urlAction: '',
        params: {}
    }

    this.init = function (url, params, callback, confirm, method, returnType, alertMessageDefault) {
        this.vars.urlAction = url;
        this.vars.params = params;
        this.vars.callback = (callback == '' || callback == undefined ? null : callback);
        this.vars.alertMessageDefault = (alertMessageDefault === false ? false : true);
        this.vars.method = (method == '' || method == undefined ? 'post' : method);
        this.vars.returnType = (returnType == '' || returnType == undefined ? 'json' : returnType);
        
        if (confirm != null && confirm != 'undefined') {
            fYes = function () { Ajax.send(); };
            fNo = function () { return false; };
            jM.confirmation(confirm, fYes, fNo);
        } else {
            this.send();
        }
    }

    this.send = function () {
        var options = this.vars;
        $.ajax({
            type: this.vars.method,
            data: this.vars.params,
            url: this.vars.urlAction,
            dataType: this.vars.returnType,
            traditional: true,
            success: function (data) {
                //Se n�o houver fun��o callback atribui-se "false"
                if (options.callback == null) {
                    options.callback = function (data) { return false; };
                }
                //alert((options.alertMessageDefault == true));
                if (options.alertMessageDefault === true) {
                    if (data == null) {
                        options.callback(data);
                    } else {
                        if ((data.error != 'undefined' && data.error == true) || (data.flagError != 'undefined' && data.flagError == true)) {

                            if (typeof (data.message) != 'undefined') {
                                jM.error(data.message);
                            } else if (data.listaErros != undefined && data.listaErros.length > 0) {
                                jM.error(data.listaErros.join("<br/>"));
                            } else {
                                jM.error("Erro durante a opera&ccedil;&atilde;o");
                            }
                        } else {
                            //Se houver mensagem de retorno abre-se o dialog e depois executa a fun��o callback, caso contr�rio apenas dispara a fun��o
                            if (typeof (data.message) != 'undefined') {
                                jM.success(data.message);
                            } else if (data.listaErros != undefined && data.listaErros.Length > 0) {
                                jM.success(data.listaErros.join("<br/>"));
                            }
                            options.callback(data);
                        }
                    }
                } else {
                    options.callback(data);
                }
            },
            error: function (err) {
                console.dir(err);
                //alert(err.message);
                //jM.error('ERROR '+err.toString());
            }
        });
    };


    //
    this.updateAjaxList = function (element, url, params) {
        var func = function (data) {
        	$(element).html(data);
        	$(element).find("input:text").setMask();
        	DefaultSistema.iniciarLinksAcao();
        	try {
        		$(element).find(".datepicker").datepicker({
        			format: "dd/mm/yyyy",
        			todayBtn: "linked",
        			language: "pt-BR",
        			orientation: "bottom left",
        			autoclose: true,
        			todayHighlight: true
        		});
        	} catch (e) {
        	}
        };
        var Assync = new ObjAjax();
        Assync.init($("#baseUrlGeral").val() + url, params, func, null, "post", "html", false);
    };


}

var Ajax = new ObjAjax();
function DefaultAction() {
    var element;
    var config = {
        element: null,
        idAffected: null,
        urlAction: ''
    }
    
    /**
     * Submete o formulário de filtros juntamente com informações de paginação.
     */
    this.submitFilterPaginate = function(page){
        $("#searchFilters input[name=page]").val(page);
        $("#searchFilters").submit();
    }
    
    /**
     * Recebe os pedidos de requisição a partir de elementos HTML configurados
     */
    this.action = function (objSource, action) {
        element = $(objSource);
        config.idAffected = element.attr("data-id");
        config.urlAction = element.attr("data-url");

        if (element.attr("data-id") == '' || element.attr("data-id") == undefined) {
            alert('Error. This element does not have attr "cod"!');
            return false;
        }

        switch (action) {
            case 'delete':
                remove();
                break;
            case 'deleteFile':
                removeFile();
                break;
            default:
                updateStatus();
                break;
        }
        return false;
    }

    /**
     * Efetiva a requisição
     */
    this.exec = function(func, confirm){
        Ajax.init(config.urlAction, {
            id: config.idAffected
        }, func, confirm );
    }

    
    //Atualização de Status Padrão
    var updateStatus = function () {
        var func = function (data) {
            console.log(data);
            switch (data.active) {
                case "S":
                    element.removeClass("text-red").addClass("text-green").html("<i class='fa fa-check'></i> Ativo");
                    break;
                case "N":
                    element.removeClass("text-green").addClass("text-red").html("<i class='fa fa-times'></i> Desativado");
                    break;
            }

            var fnCallback = $(element).attr("data-fncallback");

            if (fnCallback != null) {
                eval(fnCallback);
            }
        };

        DefaultAction.exec(func, Vocabulary.confirmUpdateStatus);
    };  // end updateStatus


    /**
     * Remoção de Itens padrão
     */
    var remove = function () {
        var func = function () {
            $(element).parent("td").parent("tr").hide("slow");
            $(element).parent("div").parent("li").hide("slow");

            $(element).closest(".box-info-item").hide("slow");
            $(element).closest(".box-info-item-dotted").hide("slow");

            var fnCallback = $(element).attr("data-fncallback");

            if (fnCallback != null) {
                eval(fnCallback);
            }

        };
        DefaultAction.exec(func, Vocabulary.confirmDelete);
    }; // End remove

	/**
    *
    */
    this.removeAll = function (element) {
    	var postData = { 'id': [] };
    	$("input[type=checkbox][name='checkRegistro[]']:checked").each(function () {
    		postData["id"].push($(this).val());
    	});
    	var url = $(element).attr("data-url");
    	if (postData["id"].length == 0) { jM.info("Selecione ao menos um registro."); return false; }

    	var func = function () {
    		location.reload();
    	};
    	Ajax.init(url, postData, func, Vocabulary.confirmDelete);
    };
        
    /**
    * Remoção padrão para Arquivos
    */
    var removeFile = function () {
        var func = function () {
            location.reload();
        };
        DefaultAction.exec(func, Vocabulary.confirmDelete);
    }; // End remove
       
    //
    this.gerarExcel = function () {

        var postData = new Array();

        $("input[type=checkbox][name='checkRegistro[]']:checked").each(function () {
            postData.push($(this).val());
        });

        if (postData.length == 0) {
            jM.info("Selecione ao menos um registro.");
            return false;
        }

        $("#idsExcel").val(postData);
        $("#formExcel").submit();

        return false;
    };
}

var DefaultAction = new DefaultAction();
function DefaultSistemaClass() {

	this.init = function () {
	    
	    //IniciarMenu
	    Nav.iniciarMenuLateral();

	    Nav.iniciarMenuTopo();
	    
	    //Plugins
        this.iniciarPlugins();

	    //
        this.iniciarLinksAcao();

	    //
        this.iniciarAutoConteudo();

	    //
        this.iniciarTimersConteudo();

        //
        this.iniciarTabs();

        //
        this.iniciarLinhaLink();

	    //
	    this.iniciarCheckBoxes();

	    //
        this.iniciarDropAutomatic();
	};

    // Iniciar verificação de posição a ser executado o drop
	this.iniciarDropAutomatic = function () {
        $(document).on("shown.bs.dropdown", ".dropdown", function () {

            // calculate the required sizes, spaces
            var $ul = $(this).children(".dropdown-menu");
            var $button = $(this).children(".dropdown-toggle");
            var ulOffset = $ul.offset();
            // how much space would be left on the top if the dropdown opened that direction
            var spaceUp = (ulOffset.top - $button.height() - $ul.height()) - $(window).scrollTop();
            // how much space is left at the bottom
            var spaceDown = $(window).scrollTop() + $(window).height() - (ulOffset.top + $ul.height());
            // switch to dropup only if there is no space at the bottom AND there is space at the top, or there isn't either but it would be still better fit
            if (spaceDown < 0 && (spaceUp >= 0 || spaceUp > spaceDown))
                $(this).addClass("dropup");
        }).on("hidden.bs.dropdown", ".dropdown", function() {
            // always reset after close
            $(this).removeClass("dropup");
        });
    }

    // Iniciar e tratar linhas de tabelas que possuam func��o de link e configura��o da a��o do clique
    this.iniciarLinhaLink = function () {
        $("tr.link td").on("click", function () {
            var action = $(this).parent().attr("data-action");
            location.href = (action);
        });
    };

    //Quando um checkbox com name=marcarTodos for clicado, deve marcar/desmarcar os checkboxes filhos
    this.iniciarCheckBoxes = function() {

        $("#checkMarcarTodos").on("click", function () {
            var status = $(this).is(":checked");

            var selectorFilho = $(this).data("childs");

            $("input[name='" + selectorFilho + "']").each(function () {
                $(this).prop("checked", status);
            });
        });

        $(".checkMarcarTodos").each(function () {

            $(this).on("click", function () {
                var status = $(this).is(":checked");

                var selectorFilho = $(this).data("childs");

                $("input[name='" + selectorFilho + "']").each(function () {
                    $(this).prop("checked", status);
                });
            });

        })
        
    };


    // Iniciar os plugins diversos do sistema usando as bibliotecas
    this.iniciarPlugins = function (conteudo) {

        if (!conteudo) {
            conteudo = $("body");
        }

        $(conteudo).find('input:text').setMask();

        this.iniciarComplementoMask();

        DefaultSistema.iniciarBotoes(conteudo);

        //Iniciar JQuery.Loading
        if (typeof ($.fn.loadingOverlay) == 'function') {
            $.fn.loadingOverlay.defaults.loadingText = "Carregando";
        }

        //
        if (typeof ($.fn.fileinput) != 'undefined') {
            var $input = $(conteudo).find('input.file[type=file]'), count = $input.attr('type') != null ? $input.length : 0;
            if (count > 0) {
                $input.fileinput();
            }
        }

        if (typeof ($.fn.tooltip) != 'undefined') {
            $(conteudo).find("[data-toggle=tooltip]").not("[data-original-title]").tooltip({
                container: 'body'
            });
        }

        if (typeof (AppAutoComplete) != 'undefined' && typeof (AppAutoComplete.iniciarSelect2) != 'undefined') {
            AppAutoComplete.iniciarSelect2($(conteudo));
        }
        
        if (typeof ($.fn.multiselect) != 'undefined') {
            $(".input-multiselect").multiselect({
                buttonClass: 'btn btn-sm btn-default', numberDisplayed: 1
            });
        }

        if (typeof (iniciarDatePicker) != 'undefined') {
            iniciarDatePicker();
        }

        if (typeof (iniciarDateTimePicker) != 'undefined') {
            iniciarDateTimePicker();
        }

        this.zerarErros();
    };

    //
    this.iniciarBotoes = function (conteudo) {

        $(conteudo).find(".link-loading").button('reset');
        $(conteudo).find(".link-loading").on('click', function () {
            var btn = $(this).button().data('loading-text', 'Processando...');
            btn.button('loading');
        });

        $(conteudo).find(".link-loading-min").button('reset');
        $(conteudo).find(".link-loading-min").on('click', function () {
            var btnMin = $(this).button().data('loading-text', "<i class=\"far fa-spin fa-sync\"></i>");
            btnMin.button('loading');
        });

    };

    //Configuracao de acoes padrao para links pre-configurados no sistema para exclusoes e alteracoes de status no sistema.
    this.iniciarLinksAcao = function (conteudo) {
                
        if (!conteudo) {
            conteudo = $("body");
        }

        DefaultSistema.iniciarLinhaLink();

        //Ouvinte do evento de troca de status de registro
        $(conteudo).find("a.ico-status").on('click', function () {
            DefaultAction.action(this, 'updateStatus');
            return false;
        });

        //Ouvinte do evento de exclusao dos registros
        $(conteudo).find("a.delete-default").on('click', function () {
            DefaultAction.action(this, 'delete');
            return false;
        });
    };

    //responsavel pela requisiao get e abertura do modal
    this.showModal = function (urlContent, funcSuccess) {
        
        $.ajax({

            url: urlContent, 
            type: 'GET',
            cache: false, 
            contentType: false, 
            processData: false,
            success: function (data) {

                DefaultSistema.exibirModal(data, funcSuccess);                
                
            },
            complete: function(xhr, textStatus) {
                
                if (xhr.status == 403) {

                    DefaultSistema.exibirModal(xhr.responseText);
                    
                }
                
            }
            
        })
        
    };
    
    this.exibirModal = function(data, funcSuccess) {

        if ((data.flagErro != "undefined" && data.flagErro == true)) {
            if (data.message) {
                jM.error(data.message);
            } else {
                jM.error("N&atilde;o foi poss&iacute;vel realizar a opera&ccedil;&atilde;o");
            }

            DefaultSistema.reiniciarBotao();
            return false;
        }

        var Modal = $(data).modal();

        $(Modal).on("shown.bs.modal", function (e) {

            DefaultSistema.reiniciarBotao();

            $('input:text').setMask();

            if (typeof (iniciarDatePicker) != 'undefined') {
                iniciarDatePicker();
            }

            if (typeof(AppAutoComplete) != 'undefined' && typeof(AppAutoComplete.iniciarSelect2) != 'undefined') {
                AppAutoComplete.iniciarSelect2(Modal);
            }

            $("[data-toggle=tooltip]").tooltip({
                container: 'body'
            });

            if (typeof ($.fn.fileinput) != 'undefined') {
                var $input = $(Modal).find('input.file[type=file]'), count = $input.attr('type') != null ? $input.length : 0;
                if (count > 0) {
                    $input.fileinput();
                }
            }

            DefaultSistema.iniciarBotoes(Modal);

            if (typeof funcSuccess !== 'undefined' && $.isFunction(funcSuccess)) {
                funcSuccess();
            }

        });

        $(Modal).on("hidden.bs.modal", function (e) {
            $(this).remove();
        });
        
    }

    //
    this.removerModais = function () {

        $(".modal").remove();

        $(".modal-backdrop").remove();

    };

    //Iniciar o plugin de tabs do bootstrap
    this.iniciarTabs = function () {

        //save the latest tab; use cookies if you like 'em better:
        $('a[data-toggle="tab"]').on('click', function () {
            localStorage.setItem('lastTab', $(this).attr('href'));
        });

        //Ao abrir um aba carregar as div com conte�do din�mico para exibicao
        $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {

            var target = $(e.target).attr("href"); // activated tab
            DefaultSistema.iniciarAutoConteudo($(target));

        });

        //go to the latest tab, if it exists:
        var lastTab = localStorage.getItem('lastTab');
        if (lastTab) {
            $('a[href="' + lastTab + '"]').tab('show');
        } else {
            // Set the first tab if cookie do not exist
            $('a[data-toggle="tab"]:first').tab('show');
        }

        //Tratamento para voltar para a primeira aba ap�s usar a p�gina de edi��o.
        if (window.location.pathname.toString().indexOf("listar") != -1) {
            localStorage.setItem('lastTab', false);
        }


    };


    //Carregar conteudo dinamicamente via ajax
    this.iniciarTimersConteudo = function () {
        var secondsTimer = 600;
        var miliTimer = (secondsTimer * 1000);

        DefaultSistema.carregarBlocoDinamico();

        setInterval(function () {
            DefaultSistema.carregarBlocoDinamico();
        }, miliTimer);
    };


    //Carregar conteudo dinamicamente via ajax e que deve ser atualizado automaticamente.
    this.carregarBlocoDinamico = function () {
        $(".box-dinamico").each(function () {
            DefaultSistema.carregarConteudo($(this));
        });
    };


    //Carregar conteudo dinamicamente via ajax apenas no carregamento da pagina
    this.iniciarAutoConteudo = function (conteudo) {
        if (!conteudo) {
            conteudo = $("body");
        }

        $(conteudo).find(".content-load:visible").each(function () {
            DefaultSistema.carregarConteudo($(this));
        });
    };

    //
    this.carregarConteudoElemento = function (idElemento) {
        this.carregarConteudo($("#" + idElemento));
    };

    //
    this.carregarConteudo = function (element, fnCallback) {
        var div = $(element);
        
        var url = div.attr("data-url");
        var targetId = div.attr("data-target");
        var divAlvo = $("#" + targetId);
        
        if (typeof (targetId) == 'undefined' || targetId == '') {
            divAlvo = $(div);
        }

        if (fnCallback == null) {
            fnCallback = div.data("fncallback");
        }

        $.ajax({
            url: url, type: "GET", cache: false,
            success: function (response) {

                divAlvo.html(response);

                $(div).removeClass("carregando");

                $(divAlvo).removeClass("carregando");

                DefaultSistema.iniciarPluginsAposAjax(divAlvo);

                if (divAlvo.find(".treeview").length > 0) {
                    $(".sidebar .treeview").tree();
                };
                
                if (fnCallback != null) {
                    if ($.isFunction(fnCallback)) {
                        fnCallback();
                    } else {
                        eval(fnCallback);
                    }
                }
            },
            error: function (response, e, h) {
                div.html(response);
            }
        });
    };

    //Reiniciar os plugins apos um bloco ser carregado via ajax
    this.iniciarPluginsAposAjax = function (conteudo) {
        //console.log(conteudo);

        DefaultSistema.iniciarPlugins(conteudo);

        DefaultSistema.iniciarLinksAcao(conteudo);
    };


    //Limpar os valores dos campos do formulario para novas insercoes
    this.limparForm = function (oForm) {
        $(oForm).find("input[type!=button][type!=submit]:not(.nao-limpar)").val('');
        $(oForm).find('select:not(.nao-limpar)').val('');
        $(oForm).find('textarea:not(.nao-limpar)').val('');
    };

	//
    this.customValidators = function () {
    	$.validator.methods.date = function (value, element) {
    		return this.optional(element) || Globalize.parseDate(value, 'dd/MM/yyyy') !== null;
    	}
    	$.validator.methods.number = function (value, element) {
    		return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value);
    	};
    };

    //
    this.reiniciarBotao = function (conteudo) {

        if (!conteudo) {
            conteudo = $("body");
        }

        setTimeout(function () {
            DefaultSistema.iniciarBotoes(conteudo);
        }, 500);

    }

    //
    this.zerarErros = function () {

        $(".input-validation-error").on("focus", function () {
            $(this).removeClass("input-validation-error");

            $(this).siblings(".field-validation-error").hide();
        });
    };

    //
    this.iniciarComplementoMask = function () {

        $("input[alt=phone]").on('keyup', function () {
            if ($(this).val().length > 14) {
                $(this).setMask("(99) 99999-9999");
            } else {
                $(this).setMask({ mask: "(99) 9999-99999", autoTab: false });
            }
        });

        $("input[alt=cnpf]").not(".cnpf-ajustado").each(function () {
            if ($(this).val().length > 11) {
                $(this).setMask({ mask: "99.999.999/9999-99", autoTab: false });
            } else {
                $(this).setMask({ mask: "999.999.999-999", autoTab: false });
            }

            $(this).addClass("cnpf-ajustado");
        });

    };

    // Extensao $.serialize
    this.serializeExtensions = function () {
        $.fn.serializeObject = function () {
            var o = {};
            var a = this.serializeArray();
            $.each(a, function () {
                if (o[this.name] !== undefined) {
                    if (!o[this.name].push) {
                        o[this.name] = [o[this.name]];
                    }
                    o[this.name].push(this.value || '');
                } else {
                    o[this.name] = this.value || '';
                }
            });
            return o;
        };
    };

    this.alterarVisaoUnidade = function (element) {
        
        var idUnidade = $(element).val();

        var callback = function (data) {
            location.reload(true);
        };
        
        var Assync = new ObjAjax();
        Assync.init($("#baseUrlGeral").val() + 'Unidades/Unidade/loadVisaoUnidade', { "idVisaoUnidade": idUnidade }, callback, null, 'post', 'json', false);
    };

    //Recarregar o sistema de modo que o usuario logado seja vinculado e uma transportadora
    this.alterarVisaoAssociacao = function (element) {

        var idOrganizacao = $(element).val();

        var callback = function (data) {

            Nav.zerarMenu();

            CarregadorPermissao.carregarPermissoes();

            CarregadorPermissao.carregarPermissoesTopo();

            CarregadorPermissao.redirecionar('#');

        };

        var Assync = new ObjAjax();

        Assync.init($("#baseUrlGeral").val() + 'associacoes/associacao/loadVisaoAssociacao', { "idOrganizacaoVisao": idOrganizacao }, callback, null, 'post', 'json', false);
    };

    this.checkNull = function (value, t) {
        t = !t || typeof t === "undefined" ? "" : t;
        return (!value || typeof value === "undefined" ? t : value);
    };

    this.getGroupCod = function (group) {
        return (!group || typeof group === "undefined" ? "" : "[data-group=" + group + "]");
    };

    this.limparCampos = function (conteudo) {

        if (!conteudo) {
            conteudo = $("body");
        }

        $(conteudo).find("input[type=text]").val('');

        $(conteudo).find("select").each(function() {
        
            var combo = $(this);
                
            combo.val("").trigger("change");

        })

    };

    //Ocultar o menu principal
    this.hideMenuPrincipal = function () {

        $("body").addClass("sidebar-collapse");
    };
};

var DefaultSistema = new DefaultSistemaClass();


$(document).ready(function () {
    
    DefaultSistema.init();
    DefaultSistema.customValidators();
    DefaultSistema.serializeExtensions();
});

function ComboSelect(){
    
    this.init = function(){
        //$("select").selectBox();   
        ComboSelect.checkEditSelect();
    };
    
    //
    this.checkEditSelect = function(){
        //$('select[edit=false]').not("[value='']").selectBox("disable");
        //$('form[edit=false] select').selectBox("disable");
    };
    
     /**
     * Caso o elemento esteja configurado com plugin selectbox, faz a reinicializa��o do mesmo para pegar as mudan�as do combo
     */
    this.resetSelectBox = function(element, enable){
        if(element.hasClass('selectBox')){
            element.selectBox("destroy");
            element.selectBox();
            if(enable){
                element.selectBox('enable');
            }else{
                element.selectBox('disable');
            }
        }
    }

     /**
     * Preenchimento de itens de um combo de sele��o
     */
    this.loadSelectBox = function(combo, rows, selectedItem){
        combo.find("option").not("option:first").remove();
        combo.find("option:first").html(Vocabulary.loading);
        combo.find("option:first").removeAttr("selected");

        //ComboSelect.resetSelectBox(combo, false);
        var selected = false;
        $.each(rows, function(key, item){
            selected = (item.id == selectedItem? true: false);
            combo.append(new Option(item.name, item.id, selected));
        });
        combo.find("option:first").html(Vocabulary.select);

        //ComboSelect.resetSelectBox(combo, true);
    }

};

var ComboSelect = new ComboSelect();
$(document).ready(function(){
    ComboSelect.init();
});

function CarregadorPermissaoClass() {

    //
    this.init = function () {
    };
    
    //Redirecionar
    this.redirecionar = function(urlRedirecionamento) {

        var timer = null;

        timer = window.setInterval(function () {

            if (Nav.flagMenuTopo == true && Nav.flagMenuLateral) {

                console.log("redirecionamento de usuario...");

                window.clearInterval(timer);

                if(urlRedirecionamento == '#'){
                    location.reload(true);                    
                }else{
                    location.href = urlRedirecionamento;
                }

                return;
            }
            console.log("Aguardando autorizacao de modulos...");

        }, 1000);        
    };

    //Carregar as opcoes para as quais o usu�rio tem acesso
    this.carregarPermissoes = function() {

        var url = new String($("#baseUrlGeral").val()).concat("permissao/menu/exibir-menu-principal");

        $(".login-processando label").html("Carregando permiss&otilde;es...");

        $.get(url, {}, function (response) {
            
            Nav.criarMenuLateral(response);

        });

    }

    //Carregar as opcoes para as quais o usu�rio tem acesso
    this.carregarPermissoesTopo = function () {

        var url = new String($("#baseUrlGeral").val()).concat("permissao/menu/exibir-menu-topo");

        $(".login-processando label").html("Liberando menus e m&oacute;dulos...");

        $.get(url, {}, function (response) {

            Nav.criarMenuTopo(response);

            return;

        });

    }


};

var CarregadorPermissao = new CarregadorPermissaoClass();

$(document).ready(function () {
    CarregadorPermissao.init();
});
function NavClass() {

    var keyNavMenuLateral = "lcNavsl_associatec";
    var keyNavMenuTopo = "lcNavTp_associatec";
    var keySelectedMenu = "lcMSelsl_associatec";
    this.flagMenuTopo = false;
    this.flagMenuLateral = false;

    //
    this.init = function () {
        $('.dropdown-menu .user-body').click(function (e) {
            e.stopPropagation();
        });
    };

    //Definir o conteudo de um menu
    this.criarMenuLateral = function (menu) {
        localStorage.setItem(keyNavMenuLateral, menu);
        Nav.flagMenuLateral = true;
    };

    //Definir o conteudo de um menu
    this.criarMenuTopo = function (menu) {
        localStorage.setItem(keyNavMenuTopo, menu);
        Nav.flagMenuTopo = true;
    };


    //Inserir o html do menu na pagina
    this.iniciarMenuLateral = function () {

        var htmlMenu = localStorage.getItem(keyNavMenuLateral);
        
        $("#boxMenu").html(htmlMenu);

        //Enable sidebar tree view controls
        $.AdminLTE.tree('.sidebar');

        var o = $.AdminLTE.options;

        //Enable control sidebar
        if (o.enableControlSidebar) {
            $.AdminLTE.controlSidebar.activate();
        }

        this.marcarMenu();

/*
        if (o.navbarMenuSlimscroll && typeof $.fn.slimscroll != 'undefined') {
            $(".navbar .menu").slimscroll({
                height: o.navbarMenuHeight,
                alwaysVisible: true,
                size: o.navbarMenuSlimscrollWidth
            }).css("width", "100%");
        }
*/

    };


    //Inserir o html do menu na pagina
    this.iniciarMenuTopo = function () {

        var htmlMenu = localStorage.getItem(keyNavMenuTopo);

        $("#boxMenuTopo").html(htmlMenu);

    };


    //Excluir menu
    this.zerarMenu = function () {

        localStorage.setItem(keyNavMenuLateral, null);

        localStorage.setItem(keyNavMenuTopo, null);

        localStorage.setItem(keySelectedMenu, null);
    };


    //Salvar o ultimo menu clicado para manter a marcacao e aberto se houver submenus
    this.selectMenu = function (elemento) {

        var idGrupo = $(elemento).data("id-grupo");

        localStorage.setItem(keySelectedMenu, idGrupo);
    };


    //Deixar o ultimo menu visitado marcado
    this.marcarMenu = function () {

        var idGrupo = localStorage.getItem(keySelectedMenu);

        if (idGrupo === "null") {

            $("#menu_0").addClass("active");

            return;
        }

        var item = $("#menu_" + idGrupo+" a");
        
        item.addClass("active");

        item.trigger('click');
    }
};

var Nav = new NavClass();

$(document).ready(function () {
    Nav.init();
});

function NotificacaoClass() {

    this.init = function () {

        this.carregarWidgetsTopo();

    };


    //Carregar os dados widgets do topo
    this.carregarWidgetsTopo = function () {

        var idBoxTarefa = $("#widgetTarefas");

        var urlTarefa = idBoxTarefa.data("url");

        $.get(urlTarefa, {}, function (response) {
            idBoxTarefa.html(response);
        });

        var idBoxAvisos = $("#widgetNotificacoes");

        var urlAvisos = idBoxAvisos.data("url");

        $.get(urlAvisos, {}, function (response) {
            idBoxAvisos.html(response);
        });

    }

    //Registrar a leitura do aviso
    this.registrarLeitura = function (id) {

        var fYes = function () {

            var idBoxAvisos = $("#widgetNotificacoes");

            var url = $("#baseUrlGeral").val() + 'notificacoes/NotificacaoWidget/registrar-leitura';

            $.post(url, { 'id': id }, function (response) {

                idBoxAvisos.html(response);

            });

        };

        //
        var fNo = function () {
            return false;
        };

        jM.confirmation("A mensagem não será mais exibida. Confirma a leitura?", fYes, fNo);
       
    };

}

var Notificacao = new NotificacaoClass();

$(document).ready(function () {
    Notificacao.init();  
});


function PesquisaRapidaClass() {

    var campoBuscaRapida = "#campoBuscaRapida";
    var boxResultadoPesquisa = "#boxResultadoPesquisa";
    var boxPesquisaRapidaResultados = "#boxPesquisaRapidaResultados";

    this.init = function () {
        this.pesquisaRapida();
    };

    // Inicar o box do header do sistema que disponibiliza formulário para buscas rápidas
    this.pesquisaRapida = function () {

        $(".consulta-rapida").submit(function (e) {
            e.preventDefault();
            
            PesquisaRapida.enviarPesquisaRapida($("#campoBuscaRapida"));
        })

    }

    //
    this.enviarPesquisaRapida = function (elemento) {

        var textoBusca = $(elemento).val();

        if (textoBusca.length < 1) {
            
            if ($("#boxPesquisaRapida").is(":visible")) {
                $("#boxPesquisaRapida").hide("fast");
            }

            return false;

        }

        if ($("#boxPesquisaRapida").is(":hidden")) {
            $("#boxPesquisaRapida").show("fast");
        }

        $(boxPesquisaRapidaResultados).html("<div class='padding-10 text-center'><i class=\"fa fa-spin fa-sync\"></i> Localizando registros...</div>");

        var url = new String($("#baseUrlGeral").val()).concat("Associados/associadobusca/pesquisa-rapida-listar?valorBusca=").concat(textoBusca);
        $.get(url, {}, function (response) {
            
            $(boxPesquisaRapidaResultados).html(response);

            $(boxResultadoPesquisa).slimScroll({
                height: 300,
                alwaysVisible: false
            });

            DefaultSistema.iniciarPluginsAposAjax($(boxResultadoPesquisa));

        });
    }

};

var PesquisaRapida = new PesquisaRapidaClass();

$(document).ready(function () {
    PesquisaRapida.init();
});