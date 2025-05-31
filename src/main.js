// src/main.js

import StartScene from './StartScene.js';
import OptionsScene from './OptionsScene.js';

let currentScene = null;
const rootId = 'ui-root';

function loadStartScene() {
    if (currentScene && typeof currentScene.dispose === 'function') {
        currentScene.dispose();
    }
    currentScene = new StartScene({
        rootId,
        onStart: () => {
            console.log('START GAME gedrückt! (hier kommt irgendwann gameplay oder so)');
        },
        onOptions: () => {
            loadOptionsScene();
        }
    });
    currentScene.init();
}

function loadOptionsScene() {
    if (currentScene && typeof currentScene.dispose === 'function') {
        currentScene.dispose();
    }
    currentScene = new OptionsScene({
        rootId,
        onBack: () => {
            loadStartScene();
        }
    });
    currentScene.init();
}

window.addEventListener('DOMContentLoaded', () => {
    loadStartScene();

    function loop() {
        if (currentScene && typeof currentScene.update === 'function') {
            currentScene.update();
        }
        requestAnimationFrame(loop);
    }
    requestAnimationFrame(loop);
});
