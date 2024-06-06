#include "../include/spectacularAI/unity/replay.hpp"
#include "../include/spectacularAI/unity/mapping.hpp"

#include <cstring>
#include <cassert>
#include <memory>
#include <spectacularAI/replay.hpp>
#include <stdexcept>

spectacularAI::Replay* sai_replay_build(
        const char* folder,
        const char* configurationYAML,
        callback_t_mapper_output onMapperOutput,
        char* errorMsg) {
    try {
        spectacularAI::Vio::Builder vioBuilder = spectacularAI::Vio::builder();
        vioBuilder.setConfigurationYAML(configurationYAML);
        if (onMapperOutput) {
            vioBuilder.setMapperCallback(
                [onMapperOutput](spectacularAI::mapping::MapperOutputPtr mappingOutput) {
                    const MapperOutputWrapper* wrapper = new MapperOutputWrapper(mappingOutput);
                    onMapperOutput(wrapper);
                }
            );
        }

        return spectacularAI::Replay::builder(folder, vioBuilder).build().release();
    } catch(const std::runtime_error &e) {
        if (errorMsg != nullptr) {
            strncpy(errorMsg, e.what(), 1000 - 1);
            errorMsg[1000 - 1] = '\0'; // Ensure null-termination
        } else {
            throw e;
        }
    }

    return nullptr;
}

void sai_replay_release(spectacularAI::Replay* replayHandle) {
    if (replayHandle) delete replayHandle;
}

void sai_replay_start(spectacularAI::Replay* replayHandle) {
    assert(replayHandle);
    replayHandle->startReplay();
}

void sai_replay_run(spectacularAI::Replay* replayHandle) {
    assert(replayHandle);
    replayHandle->runReplay();
}

bool sai_replay_one_line(spectacularAI::Replay* replayHandle) {
    assert(replayHandle);
    return replayHandle->replayOneLine();
}

void sai_replay_set_playback_speed(spectacularAI::Replay* replayHandle, double speed) {
    assert(replayHandle);
    return replayHandle->setPlaybackSpeed(speed);
}

void sai_replay_set_dry_run(spectacularAI::Replay* replayHandle, bool isDryRun) {
    assert(replayHandle);
    return replayHandle->setDryRun(isDryRun);
}

void sai_replay_set_output_callback(spectacularAI::Replay* replayHandle, callback_t_vio_output onOutput) {
    assert(replayHandle);
    assert(onOutput);
    replayHandle->setOutputCallback(
        [onOutput](const spectacularAI::VioOutputPtr vioOutput) {
            const VioOutputWrapper* wrapper = new VioOutputWrapper(vioOutput);
            onOutput(wrapper);
        }
    );
}
