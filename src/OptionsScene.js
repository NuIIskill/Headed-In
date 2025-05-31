// src/OptionsScene.js

export default class OptionsScene {
    /**
     * @param {Object}
     * @param {string}
     * @param {Function}
     */
    constructor({ rootId, onBack }) {
        this.rootId = rootId;
        this.onBack = onBack;
        this.container = null;
    }

    init() {
        // Menü Container erstellern
        this.container = document.createElement('div');
        this.container.classList.add('menu');

        // Titel hinzufügen
        const title = document.createElement('h1');
        title.textContent = 'OPTIONS';
        this.container.appendChild(title);

        // Dummy-Text (Platzhalter)
        const dummyText = document.createElement('p');
        dummyText.textContent = 'Options';
        this.container.appendChild(dummyText);

        // BACK-Button
        const backBtn = document.createElement('button');
        backBtn.textContent = 'BACK';
        backBtn.classList.add('ui-button');
        backBtn.addEventListener('click', () => {
            if (typeof this.onBack === 'function') {
                this.onBack();
            }
        });
        this.container.appendChild(backBtn);

        document.getElementById(this.rootId).appendChild(this.container);
    }

    update() {
        // Keine Animationen aktuell weil Platzhalter
    }

    dispose() {
        if (this.container) {
            this.container.remove();
            this.container = null;
        }
    }
}
