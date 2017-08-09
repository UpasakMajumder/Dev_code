import { HEADER_SHADOW, SHOW, HIDE, CART_PREVIEW, TOGGLE } from 'app.consts';

class CartPreview {
  static togglePreview() {
    const isVisible = window.store.getState().cartPreview.isVisible;
    window.store.dispatch({
      type: CART_PREVIEW + TOGGLE,
      payload: { isVisible: !isVisible }
    });

    if (isVisible) {
      window.store.dispatch({ type: HEADER_SHADOW + HIDE });
    } else {
      window.store.dispatch({ type: HEADER_SHADOW + SHOW });
    }
  }

  constructor(button) {
    button.addEventListener('mouseenter', () => CartPreview.togglePreview());
    button.addEventListener('mouseleave', () => CartPreview.togglePreview());
  }
}

export default CartPreview;
