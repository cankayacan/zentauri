import { h, render } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Style } from "preact/jsx";
import { Button } from "../Components/Button";
import { Spinner } from "../Components/Spinner";

const backgroundStyle: Style = {
  width: "100%",
  height: "100%",
  unityBackgroundScaleMode: "ScaleAndCrop",
};

const App = () => {
  const [showRestart, setShowRestart] = useState(false);
  const [loading, setLoading] = useState(false);
  const gamePlay = require("gamePlay");

  const onGameFinished = () => setShowRestart(true);

  useEffect(() => {
    gamePlay.remove_Finished(onGameFinished);
    gamePlay.add_Finished(onGameFinished);
  }, []);

  const restartGame = () => {
    setLoading(true);
    gamePlay.RestartGame();
  };

  return (
    <div
      class="w-max h-max items-end flex-row-reverse justify-between"
      style={backgroundStyle}
    >
      {showRestart && (
        <div class="self-end w-48 pb-5 pr-5">
          <Button
            style={{
              borderRadius: [16, 16, 0, 16],
            }}
            text="Restart"
            onClick={restartGame}
            enabled={!loading}
          />
        </div>
      )}

      {loading && (
        <div class="pb-5 pl-5 grow">
          <div style={{ width: 25, marginRight: 5 }}>
            <Spinner radius={25} thickness={2.5} />
          </div>
          <div style={{ color: "#FFFFFF", fontSize: "8px" }}>Loading...</div>
        </div>
      )}
    </div>
  );
};

render(<App />, document.body);
