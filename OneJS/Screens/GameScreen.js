Object.defineProperty(exports, "__esModule", { value: true });
const preact_1 = require("preact");
const hooks_1 = require("preact/hooks");
const Button_1 = require("../Components/Button");
const Spinner_1 = require("../Components/Spinner");
const backgroundStyle = {
    width: "100%",
    height: "100%",
    unityBackgroundScaleMode: "ScaleAndCrop",
};
const App = () => {
    const [showRestart, setShowRestart] = (0, hooks_1.useState)(false);
    const [loading, setLoading] = (0, hooks_1.useState)(false);
    const gamePlay = require("gamePlay");
    const onGameFinished = () => setShowRestart(true);
    (0, hooks_1.useEffect)(() => {
        gamePlay.remove_Finished(onGameFinished);
        gamePlay.add_Finished(onGameFinished);
    }, []);
    const restartGame = () => {
        setLoading(true);
        gamePlay.RestartGame();
    };
    return ((0, preact_1.h)("div", { class: "w-max h-max items-end flex-row-reverse justify-between", style: backgroundStyle },
        showRestart && ((0, preact_1.h)("div", { class: "self-end w-56 pb-5 pr-5" },
            (0, preact_1.h)(Button_1.Button, { style: {
                    borderRadius: [16, 16, 0, 16],
                }, text: "Restart", onClick: restartGame, enabled: !loading }))),
        loading && ((0, preact_1.h)("div", { class: "pb-5 pl-5 grow" },
            (0, preact_1.h)("div", { style: { width: 25, marginRight: 5 } },
                (0, preact_1.h)(Spinner_1.Spinner, { radius: 25, thickness: 2.5 })),
            (0, preact_1.h)("div", { style: { color: "#FFFFFF", fontSize: "8px" } }, "Loading...")))));
};
(0, preact_1.render)((0, preact_1.h)(App, null), document.body);
