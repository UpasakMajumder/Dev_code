import $ from 'jquery';
import 'jquery-ui/themes/base/core.css';
import 'jquery-ui/themes/base/theme.css';
import 'jquery-ui/themes/base/datepicker.css';
import 'jquery-ui/ui/core';
import 'jquery-ui/ui/widgets/datepicker';

class Datepicker {
  constructor(input) {
    $(input).datepicker();
  }
}

export default Datepicker;
