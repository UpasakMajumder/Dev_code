class Notification {
  constructor(container) {
    this.container = container;
    this.timeForAnimation = +container.dataset.timeForAnimation;
    this.timeToRemove = +container.dataset.timeToRemove;

    const MAX_SIZE = 3;
    this.notification = {};
    for (let i = 1; i <= MAX_SIZE; i += 1) this.notification[i] = undefined;
  }

  addNotification(type) {
    const that = this;
    hideLastElement();
    changeRestElements();
    showNewNotification();

    function hideLastElement() {
      const lastNumber = Object.keys(that.notification).length;
      const element = that.notification[lastNumber];
      if (element) {
        element.classList.add('hide');
        that.notification[lastNumber] = undefined;
        setTimeout(() => {
          element.remove();
        }, that.timeForAnimation);
      }
    }

    function changeRestElements() {
      const elementNumbers = Object.keys(that.notification).length;
      for (let index = elementNumbers; index > 0; index -= 1) {
        if (index !== elementNumbers) {
          const element = that.notification[index];
          if (element) {
            element.classList.remove(`show-${index}`);
            const newIndex = index + 1;
            element.classList.add(`show-${newIndex}`);
            that.notification[newIndex] = element;
          }
        }
      }
    }

    function showNewNotification() {
      const newElement = createNewNotification();
      that.notification[1] = newElement;
      that.container.appendChild(newElement);
      window.getComputedStyle(newElement).opacity;
      newElement.classList.add('show-1');

      setTimeout(() => {
        newElement.classList.add('hide');
      }, that.timeToRemove);

      setTimeout(() => {
        newElement.remove();
      }, that.timeForAnimation + that.timeToRemove);
    }

    function createNewNotification() {
      const idealElement = that.container.querySelector(`[data-notification-type="${type}"]`);
      const newElement = idealElement.cloneNode(true);
      const closer = newElement.querySelector('.js-notification-closer');

      closer.addEventListener('click', (event) => {
        const notification = event.currentTarget.parentNode;
        notification.classList.add('hide');

        pullDownNotificationsAbove(notification);

        setTimeout(() => {
          notification.remove();
        }, that.timeForAnimation);
      });

      return newElement;
    }

    function pullDownNotificationsAbove(notification) {
      const keys = Object.keys(that.notification);
      const values = Object.values(that.notification);
      const value = values.indexOf(notification);
      const key = keys[value];
      that.notification[key] = undefined;

      Object.keys(that.notification).forEach((index) => {
        if (index > key && that.notification[index]) {
          const element = that.notification[index];
          const newIndex = index - 1;
          element.classList.remove(`show-${index}`);
          element.classList.add(`show-${newIndex}`);
          that.notification[newIndex] = element;
          that.notification[index] = undefined;
        }
      });
    }
  }
}

export default Notification;
