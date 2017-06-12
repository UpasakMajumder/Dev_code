import 'jquery-ui/themes/base/core.css';
import 'jquery-ui/themes/base/theme.css';
import 'jquery-ui/themes/base/datepicker.css';

class Datepicker {
  constructor(input) {
    $(input).datepicker(); // $ and $ui from global
  }
}

export default Datepicker;
