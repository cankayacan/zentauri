import { h, render } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Style } from "preact/jsx";
import { Button } from "../Components/Button";
import { Spinner } from "../Components/Spinner";

const backgroundStyle: Style = {
  width: "100%",
  height: "100%",
  unityBackgroundScaleMode: "ScaleAndCrop",
  backgroundImage: "images/bg.png",
};

const App = () => {
  const [loading, setLoading] = useState(false);
  const lobby = require("lobby");
  const playerRenderTexture = lobby.playerRenderTexture;
  const playerAnimator = lobby.playerAnimator;

  const wave = () => playerAnimator.SetTrigger("Wave");

  useEffect(() => {
    setInterval(() => wave(), 10000);
    wave();
  }, []);

  const startGame = () => {
    setLoading(true);
    lobby.StartGame();
  };

  return (
    <div class="relative flex-row justify-between" style={backgroundStyle}>
      <div class="absolute w-48 bottom-5 left-5">
        {loading && (
          <div class="flex-row items-center">
            <div style={{ width: 25, marginRight: "4px" }}>
              <Spinner radius={25} thickness={2.5} />
            </div>
            <div style={{ color: "#FFFFFF", fontSize: "8px" }}>Loading...</div>
          </div>
        )}
      </div>

      <image
        image={playerRenderTexture}
        class="absolute"
        style={{
          width: "100%",
          height: "100%",
          unityBackgroundScaleMode: "ScaleAndCrop",
        }}
      />

      <div class="absolute w-48 bottom-5 right-5">
        <Button
          style={{
            borderRadius: [16, 16, 0, 16],
          }}
          text="Start"
          onClick={startGame}
          enabled={!loading}
        />
      </div>
    </div>
  );
};

render(<App />, document.body);
