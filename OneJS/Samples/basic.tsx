import { h, render } from "preact"

const App = () => {
    return (
        <div>
            <label text="Select something to remove from your suitcase:" />
            <box>
                <toggle name="boots" label="Boots" value={true} />
                <toggle name="helmet" label="Helmet" value={false} />
                <toggle name="cloak" label="Cloak of invisibility" />
            </box>
            <box>
                <button name="cancel" text="Cancel" onClick={(e) => log(performance.now())} />
                <button name="ok" text="OK" />
                <textfield onInput={(e) => log(e.newData)} />
            </box>
        </div>
    )
}

render(<App />, document.body)