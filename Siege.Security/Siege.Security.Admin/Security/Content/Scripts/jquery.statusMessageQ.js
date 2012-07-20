/*
* jQuery statusmessageQ plugin
* @requires jQuery v1.4.2 or later
*
* Copyright (c) 2010 M. Brown (mbrowniebytes A gmail.com)
* Licensed under the Revised BSD license:
* http://www.opensource.org/licenses/bsd-license.php
* http://en.wikipedia.org/wiki/BSD_licenses 
*
* Versions:
* 0.5 - 2010-06-17
*       added target option; where to place status messages
*       timeout takes showtime into consideration
* 		 moved warn about old browser (waob) into its own dependant plugin
* 0.4 - 2010-02-22
*       added clear_msgs()
* 0.3 - 2010-01-10
*       restructured code per jquery plugin guidelines
*       added warn_about_old_browser()
* 0.2 - 2010-01-05
*       start timeout after show
*       show wrapper just before add messages rather than first action
*       commented code 
* 0.1 - 2009-06-29
*       initial
* 
* usage:
*  $(document).ready( function() {';
*      $.fn.statusmessageQ({"message":"message to show", "type":"info"});';
*  });
*
*/

(function ($) {
    $.fn.statusmessageQ = function (options) {

        $.fn.statusmessageQ.show_msg(options);

        return this;
    }; // end statusmessageQ()	


    $.fn.statusmessageQ.clear_msgs = function (options) {
        var opts = $.extend(true, $.fn.statusmessageQ.defaults, options);

        $('div#statusmessage_wrapper').find('div.statusmessage_icon').unbind('click');

        if (opts.quick_clear) {
            $('div.statusmessage_wrapper').html('');
        } else {
            $('div.statusmessage_wrapper div.statusmessage').each(function (index) {
                var message = $(this);
                if (opts.hide == 'fadeOut') {
                    message.fadeOut(opts.hidetime).remove();
                } else {
                    message.hide(opts.hidetime).remove();
                }
            });
        }

        if ($('div.statusmessage').length == 0) {
            $('div.statusmessage_wrapper').hide();
        }
    }; // end clear_msgs()

    // build, bind, show message 
    $.fn.statusmessageQ.show_msg = function (options) {
        var opts = $.extend(true, $.fn.statusmessageQ.defaults, options);

        if (opts.decode) {
            opts.message = decodeURIComponent(opts.message);
        }

        // check for existing wrapper; else if none, add to top of body
        var message_wrapper = $('div.statusmessage_wrapper');
        if (message_wrapper.length == 0) {
            message_wrapper = $('<div class="statusmessage_wrapper"></div>');
            var target = $(opts.target);
            if (target.length == 1) { // expected
                target.prepend(message_wrapper);
            } else if (target.length > 1) { // found more than one, so use the first
                target[0].prepend(message_wrapper);
            } else { // couldn't find target, so stick on top of page
                $(document).find('body').prepend(message_wrapper);
            }
        }

        // determine type, icon, css, etc
        var type = '';
        var close = '';
        if (opts.type == 'error') {
            type = 'statusmessage_error';
            close = $('<div class="statusmessage_error_icon statusmessage_icon"></div>');
        } else if (opts.type == 'warn') {
            type = 'statusmessage_warn';
            close = $('<div class="statusmessage_warn_icon statusmessage_icon"></div>');
        } else if (opts.type == 'info') {
            type = 'statusmessage_info';
            close = $('<div class="statusmessage_info_icon statusmessage_icon"></div>');
        } else { // unknown
            type = '';
            close = $('<div class="statusmessage_icon"></div>');
        }

        // allow msg to be cleared by clicking on icon
        close.bind('click', function (e) {
            $(this).parent('div').remove();
            if ($('div.statusmessage').length == 0) {
                message_wrapper.hide();
            }
        });

        var occurrences = '';
        if (opts.occurrences > 0) {
            occurrences = '(' + opts.occurrences + ') ';
        }

        // build msg
        var message = $('<div class="statusmessage ' + type + '">' + occurrences + opts.message + '</div>');
        message.prepend(close);

        // show wrapper; if was hidden on page load
        message_wrapper.show();

        // show new msg
        if (opts.show == 'fadeIn') {
            message_wrapper.append(message.hide().fadeIn(opts.showtime));
        } else {
            message_wrapper.append(message.hide().show(opts.showtime));
        }

        // auto clear msg, if set
        if (opts.timeout > 0) {
            setTimeout(function () {
                if (opts.hide == 'fadeOut') {
                    message.fadeOut(opts.hidetime).remove();
                } else {
                    message.hide(opts.hidetime).remove();
                }
                if ($('div.statusmessage').length == 0) {
                    message_wrapper.hide();
                }
            }, opts.showtime + opts.timeout);
        }
    }; // end show_msg()

    $.fn.statusmessageQ.defaults = {
        'type': 'info', // info|warn|error; matches css
        'message': '', // text to display
        'occurrences': 0, // number of times message was repeated; only shown if > 0
        'timeout': 0, // ms;  > 0 to have status msg automatically disappear ie growl
        'show': 'fadeIn', // fadeIn|show; animation to use on show
        'showtime': 1234, // ms
        'hide': 'fadeOut', // fadeOut|hide; animation to use on hide
        'hidetime': 1234, // ms
        'quick_clear': false, // true|false; true - clear all messages; false - clear/hide each message
        'target': 'body', // target selector to append messages too; '#target', '.target'
        'decode': true	// true|false; if msg has been uri encoded; should be safe to keep true	

    }; // end $.fn.statusmessageQ.defaults{}

})(jQuery);
