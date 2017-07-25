// <div class="js-storage" data-storage-change="true" data-storage-active="false" data-storage-key="isSidebarCollapsed" data-storage-value="true">

class Storage {
  constructor(container) {
    const { storageActive, storageKey, storageValue, storageChange } = container.dataset;

    if (storageActive === 'true') localStorage.setItem(storageKey, storageValue);
    if (storageChange !== 'true') return;

    container.addEventListener('click', () => {
      if (localStorage.getItem(storageKey) === storageValue) {
        localStorage.removeItem(storageKey);
      } else {
        localStorage.setItem(storageKey, storageValue);
      }
    });

  }
}

export default Storage;
