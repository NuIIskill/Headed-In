// src/StartScene.js

export default class StartScene {
    /**
     * @param {Object} options
     * @param {string}   options.rootId
     * @param {Function} options.onStart
     * @param {Function} options.onOptions
     */
    constructor({ rootId, onStart, onOptions }) {
        this.rootId = rootId;
        this.onStart = onStart;
        this.onOptions = onOptions;
        this.container = null;
    }

    init() { // init
        // Menü Container erstellen
        this.container = document.createElement('div');
        this.container.classList.add('menu');

        // Titel im spiel menü event
        const title = document.createElement('h1');
        title.textContent = 'Headed-In';
        this.container.appendChild(title);

        // START GAME Button event
        const startBtn = document.createElement('button');
        startBtn.textContent = 'START GAME';
        startBtn.classList.add('ui-button');
        startBtn.addEventListener('click', () => {
            if (typeof this.onStart === 'function') {
                this.onStart();
            }
        });
        this.container.appendChild(startBtn);

        // 4. OPTIONS Button
        const optionsBtn = document.createElement('button');
        optionsBtn.textContent = 'OPTIONS';
        optionsBtn.classList.add('ui-button');
        optionsBtn.addEventListener('click', () => {
            if (typeof this.onOptions === 'function') {
                this.onOptions();
            }
        });
        this.container.appendChild(optionsBtn);

        // keine ahnung was das macht hat chatgpt gemacht
        document.getElementById(this.rootId).appendChild(this.container);
    }

    update() {
        // platz für weitere logik
    }

    dispose() {
        if (this.container) {
            this.container.remove();
            this.container = null;
        }
    }
}
